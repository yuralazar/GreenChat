using System;
using System.Collections.Generic;
using System.Linq;
using GreenChat.Data.Formats;
using GreenChat.Data.Instances;
using GreenChat.Data.MessageTypes;
using GreenChat.Data.MessageTypes.RecieveArgs;
using GreenChat.Data.MessageTypes.SendArgs;
using GreenChat.DAL.Models;
using GreenChat.DAL.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GreenChat.BLL.WebSockets
{
    public class SendFormatFactory
    {
        private readonly WebSocketConnectionManager _connectionManager;

        public SendFormatFactory(WebSocketConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public SendFormat InitialInfo(ApplicationUser user,
                                    List<ApplicationUser> friends
                                    ,List<ApplicationUser> potentialFriends
                                    ,List<ChatRoom> chatRooms
                                    ,List<ChatRoomUser> chatRoomsUsers
                                    ,List<PrivateMessage> unreadPrivateMessages
                                    ,List<ChatMessage> unreadChatMessages                                            
                                    ,List<ChatRoom> potentialChats)
        {
            var friendsList = friends
                .Select(us => CreateUserInfo(us, false))
                .Union(potentialFriends
                .Select(us => CreateUserInfo(us, true)));

            return CreateSendFormat(SendActionTypes.InitialInfo,
                new InitialInfo
                {
                    User = new UserInfo(user.Id, user.Email),
                    Friends = friendsList,
                    ChatRooms = chatRooms.Select(ch => new ChatInfo(ch.ChatRoomID, ch.Name)),
                    ChatRoomsUsers = GetChatRoomsUsersArguments(chatRoomsUsers.Where(roomUser => roomUser.Confirmed)),
                    PrivateMessages = unreadPrivateMessages.Select(mess => new SendPrivateArguments
                        {                    
                            UserFrom = CreateUserInfo(mess.Sender),
                            UserTo = CreateUserInfo(mess.Receiver),
                            Message = CreateMessageInfo(mess.PrivateMessageID, mess.Date, mess.Content)
                        }
                        ),
                    ChatMessages = unreadChatMessages.Select(mess => new SendChatArguments
                        {
                            UserFrom = CreateUserInfo(mess.ChatRoomUser.User),
                            Chat = CreateChatInfo(mess.ChatRoom),
                            Message = CreateMessageInfo(mess.ChatMessageID, mess.Date, mess.Content)
                        }),                        
                    ChatRequests = potentialChats.Select(ch => new ChatRequestArguments
                        {
                            UserFrom = CreateUserInfo(ch.User),
                            Chat = CreateChatInfo(ch),
                            Users = chatRoomsUsers
                                    .Where(chatRoomUser => chatRoomUser.ChatRoom.Equals(ch))
                                    .Select(chatRoomUser => CreateUserInfo(chatRoomUser.User))                                    
                    })
                });
        }

        private IEnumerable<ChatRoomUsersArguments> GetChatRoomsUsersArguments(IEnumerable<ChatRoomUser> chatRoomsUsers)
        {
            var list = new List<ChatRoomUsersArguments>();
            ChatRoom chatRoom = null;
            ChatRoomUsersArguments chatRoomUsersArguments = null;

            foreach (var chatRoomUser in chatRoomsUsers)
            {
                if (chatRoomUser.ChatRoom != chatRoom)
                {
                    chatRoom = chatRoomUser.ChatRoom;
                    chatRoomUsersArguments = new ChatRoomUsersArguments
                    {
                        Chat = CreateChatInfo(chatRoom),
                        Users = new List<UserInfo>()
                    };
                    list.Add(chatRoomUsersArguments);
                }
                var userInfos = (IList<UserInfo>)(chatRoomUsersArguments?.Users);
                userInfos?.Add(CreateUserInfo(chatRoomUser.User));
            }

            return list;
        }

        public SendFormat ErrorMessage(string methodName, string eMessage)
        {
            return CreateSendFormat(SendActionTypes.ErrorText, $"The {methodName} method : " + eMessage);        
        }

        public SendFormat ConnectionStatus(ApplicationUser user)
        {
            return CreateSendFormat(SendActionTypes.ConnectionStatus, CreateUserInfo(user));                
        }

        public SendFormat PrivateMessage(UserInfo userfrom, UserInfo userTo, MessageInfo message, int idNew)
        {
            return CreateSendFormat(SendActionTypes.PrivateMessage,
                new SendPrivateArguments
                {
                    UserFrom = userfrom,
                    UserTo= userTo,
                    Message = message,
                    IdNew = idNew
                });
        }

        public SendFormat ChatMessage(ChatInfo chatInfo, UserInfo user, MessageInfo message, int idNew)
        {
            return CreateSendFormat(SendActionTypes.ChatMessage,
                new SendChatArguments
                {
                    UserFrom = user,
                    Message = message,
                    Chat = chatInfo,
                    IdNew = idNew
                });
        }

        public SendFormat UserFound(UserInfo userFrom, ApplicationUser searchedUser)
        {
            return CreateSendFormat(SendActionTypes.UserFound,
                new UserFoundArguments
                {
                    UserFrom = userFrom,
                    User = searchedUser == null ? new UserInfo() : CreateUserInfo(searchedUser)
                });
        }

        public SendFormat FriendRequest(UserInfo userFrom)
        {
            return CreateSendFormat(SendActionTypes.FriendRequest,
                new FriendRequestArguments
                {
                    UserFrom = userFrom
                });
        }

        public SendFormat FriendConfirmed(UserInfo userFrom, bool confirmed)
        {
            return CreateSendFormat(SendActionTypes.FriendConfirmed,
                new FriendConfirmedArguments
                {
                    UserFrom = userFrom,
                    Confirmed = confirmed
                });
        }

        public SendFormat ChatCreated(UserInfo userFrom, ChatRoom сhatRoom)
        {
            return CreateSendFormat(SendActionTypes.ChatCreated,
                new ChatCreatedArguments
                {
                    UserFrom = userFrom,
                    Chat = CreateChatInfo(сhatRoom)
                });
        }

        public SendFormat ChatRequest(ChatInfo chat, UserInfo userFrom, List<ApplicationUser> chatRoomUsers)
        {
            return CreateSendFormat(SendActionTypes.ChatRequest,
                new ChatRequestArguments
                {
                    UserFrom = userFrom,
                    Chat = chat,
                    Users = chatRoomUsers.Select(us => CreateUserInfo(us))
                });
        }

        public SendFormat ChatConfirmed(UserInfo userFrom, ChatInfo chat, bool confirmed)
        {
            return CreateSendFormat(SendActionTypes.ChatConfirmed,
                new ChatConfirmedArguments
                {
                    UserFrom = userFrom,
                    Confirmed = confirmed,
                    Chat = chat
                });
        }

        public SendFormat PrivateMessages(ApplicationUser userFrom, List<PrivateMessageInfo> messages)
        {
            return CreateSendFormat(SendActionTypes.PrivateMessages,
                new PrivateMessagesArguments
                {
                    UserFrom = CreateUserInfo(userFrom),
                    Messages = messages.Select(message =>
                        {
                            var mess = message.Message;
                            return new SendPrivateArguments
                            {
                                UserFrom = CreateUserInfo(mess.Sender),
                                UserTo = CreateUserInfo(mess.Receiver),
                                Message = CreateMessageInfo(mess.PrivateMessageID, mess.Date, mess.Content),
                                Status = message.Status
                            };
                        }
                    )
                });
        }

        public SendFormat ChatMessages(ChatInfo chatInfo, List<ChatMessage> messages)
        {
            return CreateSendFormat(SendActionTypes.ChatMessages,
                new ChatMessagesArguments
                {                    
                    Chat = chatInfo,

                    Messages = messages.Select(mess => new SendChatArguments
                    {         
                        Chat = chatInfo,
                        UserFrom = CreateUserInfo(mess.ChatRoomUser.User),
                        Message = CreateMessageInfo(mess.ChatMessageID, mess.Date, mess.Content)
                    }
                    )
                });
        }

        public SendFormat PrivateMessageStatus(PrivateStatusArguments arguments)
        {
            return CreateSendFormat(SendActionTypes.PrivateMessageStatus, arguments);
        }

        public SendFormat ChatMessageStatus(ChatStatusArguments arguments)
        {
            return CreateSendFormat(SendActionTypes.ChatMessageStatus, arguments);
        }

        private SendFormat CreateSendFormat(SendActionTypes actionType, object obj)
        {
            return new SendFormat
            {
                SendActionType = actionType,
                Arguments = JsonConvert.SerializeObject(obj, Settings)
            };
        }

        private UserInfo CreateUserInfo(ApplicationUser user, bool potentialFriend = false)
        {
            if (user == null) return null;

            return new UserInfo
            {
                Id = user.Id,
                Email = user.Email,
                Online = _connectionManager.UserIsOnline(user),
                Potential = potentialFriend
            };
        }

        public ChatInfo CreateChatInfo(ChatRoom chatRoom)
        {
            if (chatRoom == null) return null;

            return new ChatInfo
            {
                Id = chatRoom.ChatRoomID,
                Name = chatRoom.Name
            };
        }

        private static MessageInfo CreateMessageInfo(int id, DateTimeOffset date, string text)
        {
            return new MessageInfo
            {
                Id = id,
                Date = date,
                Text = text
            };
        }
    }
}
