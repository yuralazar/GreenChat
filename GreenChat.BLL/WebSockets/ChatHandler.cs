
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using GreenChat.Data.Formats;
using GreenChat.Data.MessageTypes.RecieveArgs;
using GreenChat.DAL.Interfaces;
using GreenChat.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GreenChat.BLL.WebSockets
{
    public class ChatHandler
    {
        private readonly WebSocketConnectionManager _connectionManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly SendFormatFactory _sendFormatFactory;              

        public ChatHandler(IUnitOfWork unitOfWork
                            ,ILoggerFactory loggerFactory
                            ,WebSocketConnectionManager connectionManager
                            ,SendFormatFactory sendFormatFactory) 
        {
            _connectionManager = connectionManager;
            _sendFormatFactory = sendFormatFactory;            
            _unitOfWork = unitOfWork;
            _logger = loggerFactory.CreateLogger<ChatHandler>();
        }

        private static HandlerResult CreateResult(IReadOnlyCollection<WebSocket> sockets, SendFormat message)
        {
            return new HandlerResult {Sockets = sockets, Message = message};
        }

        private static HandlerResult CreateResult(WebSocket socket, SendFormat message)
        {
            var list = new List<WebSocket>();
            var coll = new ReadOnlyCollection<WebSocket>(list);
            list.Add(socket);
            return new HandlerResult { Sockets = coll, Message = message };
        }

        private HandlerResult CreateEmptyResult()
        {
            return new HandlerResult { Sockets = null, Message = null };
        }

        //---------------------------- Connection Handlings ------------------------

        public async Task<List<HandlerResult>> OnConnected(string userEmail, WebSocket socket)
        {
            var resList = new List<HandlerResult>();
            
            var user = await _unitOfWork.UserManager.FindByEmailAsync(userEmail);
            _connectionManager.AddSocket(user, socket);

            var statusMessage = _sendFormatFactory.ConnectionStatus(user);              
            var friends = await _unitOfWork.Friends.GetFriends(user);        
            var potentialFriends = await _unitOfWork.Friends.GetPotentialFriends(user);
            var chatRooms = await _unitOfWork.ChatRoomUsers.GetChatRoomsByUser(user);
            var roomsList = chatRooms.Select(roomUser => roomUser.ChatRoom).ToList();
            var confirmedRoomsList = chatRooms.Where(roomUser => roomUser.Confirmed).Select(roomUser => roomUser.ChatRoom).ToList();
            var potentialRoomsList = chatRooms.Where(roomUser => !roomUser.Confirmed).Select(roomUser => roomUser.ChatRoom).ToList();
            var chatRoomsUsers = await _unitOfWork.ChatRoomUsers.GetChatUsersList(roomsList);
            var unreadPrivateMessages = await _unitOfWork.UnreadPrivateMessages.Get(user.Id);
            var unreadChatMessages = await _unitOfWork.UnreadChatMessages.GetByChatRoomUser(user.Id);           

            var initialMessage = _sendFormatFactory.InitialInfo(user, friends, potentialFriends, confirmedRoomsList
                                                                 , chatRoomsUsers
                                                                 , unreadPrivateMessages
                                                                 , unreadChatMessages
                                                                 , potentialRoomsList);            
            var friendsSockets = _connectionManager.GetSockets(friends);

            resList.Add(CreateResult(socket, initialMessage));
            resList.Add(CreateResult(friendsSockets, statusMessage));

            return resList;
        }

        public async Task<HandlerResult> OnDisconnected(string userEmail, WebSocket socket)
        {
            var res = new HandlerResult();
            var user = await _unitOfWork.UserManager.FindByEmailAsync(userEmail);
            await _connectionManager.RemoveSocket(user, socket);
                        
            var friends = await _unitOfWork.Friends.GetFriends(user);
            res.Message = _sendFormatFactory.ConnectionStatus(user);
            res.Sockets = _connectionManager.GetSockets(friends);            

            return res;
        }
        //----------------------------END Connection Handlings ------------------------


        //---------------------------- Message sending ---------------------------------                
        public async Task<HandlerResult> RecievePrivate(RecievePrivateArguments arguments, WebSocket socket)
        {        
            var userInfoTo = arguments.UserTo;
            var userFrom = await _unitOfWork.UserManager.FindByIdAsync(arguments.UserFrom.Id);
            var userTo = await _unitOfWork.UserManager.FindByIdAsync(userInfoTo.Id);
            var mess = await _unitOfWork.PrivateMessages.AddPrivateMessage(userFrom, userTo, arguments.Message.Text, arguments.Message.Date);                        
            arguments.Message.Id = mess.PrivateMessageID;
            await _unitOfWork.PrivateMessageStatuses.AddSentStatus(mess.PrivateMessageID, arguments.Message.Date.DateTime);
            var sockets = _connectionManager.GetSocketsByUser(userTo);            
            SendFormat message = null;
            if (sockets != null && sockets.Count != 0)
                message = _sendFormatFactory.PrivateMessage(arguments.UserFrom, arguments.Message);
       
            return CreateResult(sockets, message);
        }

        public async Task<HandlerResult> RecieveChat(RecieveChatArguments arguments, WebSocket socket)
        {
            var chatInfo = arguments.Chat;
            var userFrom = await _unitOfWork.UserManager.FindByIdAsync(arguments.UserFrom.Id);
            var usersTo = await _unitOfWork.ChatRoomUsers.GetChatUsersList(chatInfo.Id);
            usersTo.Remove(usersTo.Find(user => user.Id == userFrom.Id));
            var mess = await _unitOfWork.ChatMessages.AddChatMessage(userFrom, chatInfo.Id, arguments.Message.Text, arguments.Message.Date);
            await _unitOfWork.SaveAsync();
            arguments.Message.Id = mess.ChatMessageID;

            var sockets = _connectionManager.GetSockets(usersTo);
            var message = _sendFormatFactory.ChatMessage(chatInfo, arguments.UserFrom, arguments.Message);

            return CreateResult(sockets, message);
        }               

        public async Task<HandlerResult> SearchFriend(SearchFriendArguments arguments, WebSocket socket)
        {
            var userFrom = await _unitOfWork.UserManager.FindByIdAsync(arguments.UserFrom.Id);
            var searchedUser = await _unitOfWork.UserManager.FindByEmailAsync(arguments.SearchEmail);
            var message = _sendFormatFactory.UserFound(arguments.UserFrom, searchedUser);
            var sockets = _connectionManager.GetSocketsByUser(userFrom);

            return CreateResult(sockets, message);
        }

        public async Task<HandlerResult> AddFriend(AddFriendArguments arguments, WebSocket socket)
        {
            var userInfoTo = arguments.User;
            var userFrom = await _unitOfWork.UserManager.FindByIdAsync(arguments.UserFrom.Id);
            var userTo = await _unitOfWork.UserManager.FindByIdAsync(userInfoTo.Id);
            // if this method was invoked from a user who started frendship request
            // we just send FrendRequest to another user
            IReadOnlyCollection<WebSocket> sockets = null;
            SendFormat message = null;

            if (arguments.Initiator)
            {
                sockets = _connectionManager.GetSocketsByUser(userTo);
                message = _sendFormatFactory.FriendRequest(arguments.UserFrom);
                await _unitOfWork.Friends.AddFriend(userFrom, userTo);
                await _unitOfWork.SaveAsync();
            }
            // if user just confirming the request, then send FriendConfirmed messages to initiator
            else
            {
                sockets = _connectionManager.GetSocketsByUser(userTo);
                message = _sendFormatFactory.FriendConfirmed(arguments.UserFrom, arguments.Confirmed);
                if (arguments.Confirmed)
                {
                    await _unitOfWork.Friends.AddFriend(userFrom, userTo);                    
                }
                else
                {
                    await _unitOfWork.Friends.Delete(userTo, userFrom);                    
                }
                await _unitOfWork.SaveAsync();
            }           
            return CreateResult(sockets, message);
        }

        public async Task<HandlerResult> CreateChat(CreateChatArguments arguments, WebSocket socket)
        {
            var userFrom = await _unitOfWork.UserManager.FindByIdAsync(arguments.UserFrom.Id);
            var ñhatRoom = await _unitOfWork.ChatRooms.AddChatRoom(userFrom, arguments.ChatName);
            await _unitOfWork.ChatRoomUsers.AddChatRoomUser(ñhatRoom, userFrom);
            await _unitOfWork.SaveAsync();
            var sockets = _connectionManager.GetSocketsByUser(userFrom);
            // sending message to chat creator
            var message = _sendFormatFactory.ChatCreated(arguments.UserFrom, ñhatRoom);

            return CreateResult(sockets, message);
        }

        public async Task<HandlerResult> InviteToChat(InviteToChatArguments arguments, WebSocket socket)
        {
            var chatInfo = arguments.Chat;
            var idsList = arguments.Users.Select(usr => usr.Id).ToList();            
            var users = await _unitOfWork.Context.Users
                .Where(user => idsList.Contains(user.Id)).ToListAsync();
            await _unitOfWork.ChatRoomUsers.AddChatRoomUsers(users, chatInfo.Id);
            var chatRoomUsers = await _unitOfWork.ChatRoomUsers.GetChatRoomUsers(chatInfo.Id);
            await _unitOfWork.SaveAsync();
            var sockets = _connectionManager.GetSockets(users);
            var message = _sendFormatFactory.ChatRequest(chatInfo, arguments.UserFrom, chatRoomUsers);

            return CreateResult(sockets, message);            
        }

        public async Task<HandlerResult> ConfirmChat(ConfirmChatArguments arguments, WebSocket socket)
        {
            var chatInfo = arguments.Chat;
            var userFrom = await _unitOfWork.UserManager.FindByIdAsync(arguments.UserFrom.Id);
            var invitor = await _unitOfWork.UserManager.FindByIdAsync(arguments.Invitor.Id);

            if (arguments.Confirmed)
                await _unitOfWork.ChatRoomUsers.ConfirmChatRoomUser(chatInfo.Id, userFrom);
            else
                await _unitOfWork.ChatRoomUsers.RemoveChatRoomUser(chatInfo.Id, userFrom);

            await _unitOfWork.SaveAsync();            
            var sockets = _connectionManager.GetSocketsByUser(invitor);            
            var message = _sendFormatFactory.ChatConfirmed(arguments.UserFrom, chatInfo, arguments.Confirmed);

            return CreateResult(sockets, message);
        }

        public async Task<HandlerResult> ReadPrivateMessages(ReadPrivateMessagesArguments arguments, WebSocket socket)
        {            
            await _unitOfWork.UnreadPrivateMessages.Delete(arguments.UserFrom.Id, arguments.UserTo.Id);
            await _unitOfWork.SaveAsync();
            return CreateEmptyResult();
        }

        public async Task<HandlerResult> ReadChatMessages(ReadChatMessagesArguments arguments, WebSocket socket)
        {            
            await _unitOfWork.UnreadChatMessages.DeleteByChatRoomUser(arguments.UserTo.Id, arguments.Chat.Id);
            await _unitOfWork.SaveAsync();
            return CreateEmptyResult();
        }

        public async Task<HandlerResult> GetPrivateMessages(GetPrivateMessagesArguments arguments, WebSocket socket)
        {
            var reciever = await _unitOfWork.UserManager.FindByIdAsync(arguments.UserFrom.Id);
            var sender = await _unitOfWork.UserManager.FindByIdAsync(arguments.Sender.Id);
            var privateMessages = await _unitOfWork.PrivateMessages.GetMessagesPortionBeforeDate(sender, reciever, arguments.Count, arguments.StartDate);
            var message = _sendFormatFactory.PrivateMessages(sender, privateMessages);
            return CreateResult(socket, message);
        }

        public async Task<HandlerResult> GetChatMessages(GetChatMessagesArguments arguments, WebSocket socket)
        {            
            var chatInfo = arguments.Chat;
            var chatMessages = await _unitOfWork.ChatMessages.GetMessagesPortionBeforeDate(chatInfo.Id, arguments.Count, arguments.StartDate);
            var message = _sendFormatFactory.ChatMessages(chatInfo, chatMessages);
            return CreateResult(socket, message);
        }
    }
}