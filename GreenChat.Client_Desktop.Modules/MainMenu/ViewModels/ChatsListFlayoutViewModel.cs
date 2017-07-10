using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GreenChat.Client_Desktop.Modules.MainMenu.Views;
using GreenChat.Client_Desktop.Modules.Service.Handlers;
using GreenChat.Client_Desktop.Modules.Service.Models;
using GreenChat.Data.MessageTypes.SendArgs;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Unity;
using PrismMahAppsSample.Infrastructure.Base;
using PrismMahAppsSample.Infrastructure.Constants;
using PrismMahAppsSample.Infrastructure.Interfaces;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ViewModels
{
    public class ChatsListFlayoutViewModel : ViewModelBase
    {
        //private Chat _selectedChat;
        //public Chat SelectedChat
        //{
        //    get { return _selectedChat; }
        //    set { SetProperty(ref _selectedChat, value); }
        //}

        //private ObservableCollection<Chat> _chats;
        //public ObservableCollection<Chat> Chats
        //{
        //    get { return _chats; }
        //    set { SetProperty(ref _chats, value); }
        //}

        private ChatMess _selectedChatMess;
        public ChatMess SelectedChatMess
        {
            get { return _selectedChatMess; }
            set { SetProperty(ref _selectedChatMess, value); }
        }

        public ObservableCollection<ChatMess> _chatsMess;
        public ObservableCollection<ChatMess> _ChatsMess
        {
            get { return _chatsMess; }
            set { SetProperty(ref _chatsMess, value); }
        }

        public class ChatMess
        {
            private String _newMessages = String.Empty;
            public String _NewMessages
            {
                get { return _newMessages; }
                set => _newMessages = value;
            }

            public Chat _Chat { get; set; }

            public ChatMess(Chat chat, String newMessages)
            {
                _NewMessages = newMessages;
                _Chat = chat;
            }
        }

        private WebSocketsMessageSender _sender;
        private WebSocketsMessageHandler _handler;
        public DelegateCommand CreateChatAsyncCommand { get; set; }
        //public DelegateCommand ChatSelectedCommand { get; set; }
        public DelegateCommand ChatMessSelectedCommand { get; set; }
        public DelegateCommand InviteFriensToChatCommand { get; set; }

        public ChatsListFlayoutViewModel(WebSocketsMessageHandler ihandler, WebSocketsMessageSender isender)
        {
            _sender = isender;
            _handler = ihandler;
            _handler.InitialInfoRecieved += OnInitialInfoRecieved;
            _handler.ChatCreatedRecieved += OnChatCreatedRecieved;
            _handler.ChatConfirmedRecieved += OnChatConfirmedRecieved;
            _handler.ChatRequestRecieved += OnChatRequestRecieved;
            _handler.ChatMessageRecieved += HandlerOnChatMessageRecieved;
            CreateChatAsyncCommand = new DelegateCommand(CreateChatAsync);
            ChatMessSelectedCommand= new DelegateCommand(ChatMessSelected);
            //ChatSelectedCommand = new DelegateCommand(ChatSelected);
            InviteFriensToChatCommand = new DelegateCommand(InviteFriensToChat);
        }

        private void HandlerOnChatMessageRecieved(object sender, SendChatArguments e)//TODO Verify
        {
            var indexOfChengableChat = _ChatsMess.ToList().FindIndex(0, mess => mess._Chat.Id == e.Chat.Id);

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _ChatsMess.Insert(indexOfChengableChat,
                    new ChatMess(_sender._ChatsManager.GetById(e.Chat.Id),
                        _sender._ChatMessagesManager.CountNewByOwner(
                            _sender._ChatsManager.GetById(e.Chat.Id)).ToString()));

                _ChatsMess.RemoveAt(indexOfChengableChat + 1);
            }));
        }

        private async void CreateChatAsync()//GOOD
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService).ShowCreateChatDialogAsync("Creating Chat", "Input Name of Chat your want to create");
            }));
        }

        private async void OnInitialInfoRecieved(object sender, InitialInfo initialInfo)//GOOD
        {
           await LoadAndShowChatsAsync();
        }

        private void InviteFriensToChat()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                RegionManager.RequestNavigate(RegionNames.DialogPopupRegion, PopupNames.ModuleAPopup);
            }));
        }

        private async Task LoadAndShowChatsAsync()//Good
        {
            var chatsCollection = new ObservableCollection<Chat>();
            chatsCollection.AddRange(_handler._ChatsManager.GetAll());

            _ChatsMess = new ObservableCollection<ChatMess>();

            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (var chat in chatsCollection)
                {
                    if(_sender._ChatMessagesManager.CountNewByOwner(chat).ToString() == "0") _ChatsMess.Add(new ChatMess(chat, String.Empty));
                    else
                    {
                        _ChatsMess.Add(new ChatMess(chat, _sender._ChatMessagesManager.CountNewByOwner(chat).ToString()));
                    }
                }
            }));
        }

        private async void ChatMessSelected()//TODO Implemet UnmarkNew
        {
            if (SelectedChatMess?._Chat == null)
                return;

            _handler._ChatGlobals.SetCurrentChat(SelectedChatMess._Chat);
            if (!_handler._ChatMessagesManager.HistoryLoaded(_handler._ChatsManager.GetById(SelectedChatMess._Chat.Id)))
            {
                await _sender.SendMessageLoadCurrentChatMessages();
            }
            else if (_handler._ChatMessagesManager.HistoryLoaded(_handler._ChatsManager.GetById(SelectedChatMess._Chat.Id)))
            {
                _sender.GiveAllMessagesByChatId(SelectedChatMess._Chat.Id);
            }
            _sender._ChatMessagesManager.UnmarkNew(_sender._ChatsManager.GetById(SelectedChatMess._Chat.Id));

            var indexOfChengableChat = _ChatsMess.ToList().FindIndex(0, mess => mess._Chat.Id == SelectedChatMess._Chat.Id);

            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                //var num = _sender._ChatMessagesManager
                //    .CountNewByOwner(_sender._ChatsManager.GetById(SelectedChatMess._Chat.Id)).ToString();
                //if(num == "0")  
                _ChatsMess.Insert(indexOfChengableChat,
                    new ChatMess(_sender._ChatsManager.GetById(SelectedChatMess._Chat.Id), String.Empty));
               
                //else
                //{
                //    _ChatsMess.Insert(indexOfChengableChat,
                //        new ChatMess(_sender._ChatsManager.GetById(SelectedChatMess._Chat.Id), num));
                //}

                _ChatsMess.RemoveAt(indexOfChengableChat + 1);
            }));

            await _sender.SendMessageReadChatMessagesArguments();
        }

        private async void OnChatRequestRecieved(object sender, ChatRequestArguments chatRequestArguments)//GOOD
        {
            if (chatRequestArguments?.Chat == null || chatRequestArguments?.UserFrom == null)
                return;

            Task<MessageDialogResult> result = null;
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                result = this.Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService).ShowFriendShipRequestAsnyc("Friendship Request recieved",
                                                                            "User: " + chatRequestArguments.UserFrom.Email + " invite you to Chat: " + chatRequestArguments.Chat.Name + "!!!",
                                                                             MessageDialogStyle.AffirmativeAndNegative);
            }));
            if (result.Result == MessageDialogResult.Affirmative)
            {
                await _sender.SendMessageConfirmChatRequest(true, chatRequestArguments);

                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _ChatsMess.Add(new ChatMess(_handler._ChatsManager.GetById(chatRequestArguments.Chat.Id), String.Empty));
                }));
            }
            else if (result.Result == MessageDialogResult.Negative)
            {
                await _sender.SendMessageConfirmChatRequest(false, chatRequestArguments);
            }
        }

        private async void OnChatCreatedRecieved(object sender, ChatCreatedArguments chatCreatedArguments)//GOOD
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _ChatsMess.Add(new ChatMess(_handler._ChatsManager.GetById(chatCreatedArguments.Chat.Id), String.Empty));
                this.Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService)
                    .ShowMessageAsnyc("Chat:" + chatCreatedArguments.Chat.Name + " was created successfuly", "Now you can invite frinds to it =)");
            }));
        }

        private async void OnChatConfirmedRecieved(object sender, ChatConfirmedArguments chatConfirmedArguments)//GOOD
        {
            if (chatConfirmedArguments.Confirmed)
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                   this.Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService)
                        .ShowMessageAsnyc("Chat Alert", "User:" + chatConfirmedArguments.UserFrom.Email + " joined to Chat: " + chatConfirmedArguments.Chat.Name);
                }));
            }
            else if (!chatConfirmedArguments.Confirmed)
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService)
                        .ShowMessageAsnyc("Chat Notification", "User " + chatConfirmedArguments.UserFrom.Email + " rejected your invitation to Chat: " + chatConfirmedArguments.Chat.Name);
                }));
            }
        }
    }
}
