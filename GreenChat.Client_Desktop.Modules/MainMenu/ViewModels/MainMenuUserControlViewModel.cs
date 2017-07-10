using System;
using System.Collections.ObjectModel;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using GreenChat.Client_Desktop.Modules.Service.Clients;
using GreenChat.Client_Desktop.Modules.Service.Handlers;
using GreenChat.Client_Desktop.Modules.Service.Models;
using Prism.Commands;
using PrismMahAppsSample.Infrastructure.Base;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ViewModels
{
    public class MainMenuUserControlViewModel : ViewModelBase
    {
        #region Properties
        //List<T>
        private ObservableCollection<User> _friends;
        public ObservableCollection<User> Friends
        {
            get { return _friends; }
            set { SetProperty(ref _friends, value); }
        }

        //private ObservableCollection<Chat> _chats;
        //public ObservableCollection<Chat> Chats
        //{
        //    get { return _chats; }
        //    set { SetProperty(ref _chats, value); }
        //}

        //private ObservableCollection<PrivateMessage> _privateMessages;
        //public ObservableCollection<PrivateMessage> PrivateMessages
        //{
        //    get { return _privateMessages; }
        //    set { SetProperty(ref _privateMessages, value); }
        //}

        //private ObservableCollection<ChatMessage> _chatMessages;
        //public ObservableCollection<ChatMessage> ChatMessages
        //{
        //    get { return _chatMessages; }
        //    set { SetProperty(ref _chatMessages, value); }
        //}

        //private String _chatNameToCreate = "";
        //public String ChatNameToCreate
        //{
        //    get { return _chatNameToCreate; }
        //    set { SetProperty(ref _chatNameToCreate, value); }
        //}

        //private String _searchingFriendEmail = "";
        //public String SearchingFriendEmail
        //{
        //    get { return _searchingFriendEmail; }
        //    set { SetProperty(ref _searchingFriendEmail, value); }
        //}

        //private String _currentUserMessageToSend = "";
        //public String CurrentUserMessageToSend
        //{
        //    get { return _currentUserMessageToSend; }
        //    set { _currentUserMessageToSend = value; }
        //}

        //private String _receivedMessages = "";
        //public String ReceivedMessages
        //{
        //    get { return _receivedMessages; }
        //    set { SetProperty(ref _receivedMessages, value); }
        //}

        #endregion
        #region Improtant instances

        //private WebSocketsMessageSender _sender;
        //private WebSocketsMessageHandler _handler;
        //private WebApiClient _webApiClient;
        //public DelegateCommand PostLogOutCommand { get; set; }
        //public DelegateCommand IsUserSignedInCommand { get; set; }
        //public DelegateCommand OpenWebSocketsConnectionAsyncCommand { get; set; }
        //public DelegateCommand SearchFriendAsyncCommand { get; set; }
        //public DelegateCommand CreateChatAsyncCommand { get; set; }
        //public DelegateCommand SendMessageCommand { get; set; }
        //public DelegateCommand InviteToChatAsyncCommand { get; set; }
        //public DelegateCommand LoadAndShowFriendsCommand { get; set; }
        //public DelegateCommand LoadAndShowChatsCommand { get; set; }
        //public DelegateCommand LoadAndShowPrivateMessagesCommand { get; set; }
        //public DelegateCommand LoadAndShowChatMessagesCommand { get; set; }
        //public DelegateCommand<User> FriendSelectedCommand { get; set; }
        //public DelegateCommand<Chat> ChatSelectedCommand { get; set; }
        #endregion

        private ObservableCollection<PrivateMessage> _privateMessages;
        public ObservableCollection<PrivateMessage> PrivateMessages
        {
            get { return _privateMessages; }
            set { SetProperty(ref _privateMessages, value); }
        }

        private WebSocketsMessageSender _sender;
        private WebSocketsMessageHandler _handler;
        private WebApiClient _webApiClient;

        public DelegateCommand LoadAndShowFriendsCommand { get; set; }

        public MainMenuUserControlViewModel(WebSocketsMessageHandler ihandler,
            WebSocketsMessageSender isender,
            WebApiClient webApiClient)
        {
            ModernDialog.ShowMessage("SomeText,", "Title", MessageBoxButton.OK);
            _sender = isender;
            _handler = ihandler;
            _webApiClient = webApiClient;

            // _sender.CreateMessageOutcomingEventHandler += OnCreateMessageOutcomingEventHandler;
            //_handler.InitialInfoRecieved += OnInitialInfoRecieved;


            PrivateMessages = new ObservableCollection<PrivateMessage>
            {
                new PrivateMessage
                {
                    Content = new MessageContent { Date = DateTime.Now, Id = 1, Text = "Message messasge message"},
                    Incoming = true,
                    IsNew = false
                },
                new PrivateMessage
                {
                    Content = new MessageContent { Date = DateTime.Now, Id = 1, Text = "Message messasge message"},
                    Incoming = false,
                    IsNew = false
                },
                new PrivateMessage
                {
                    Content = new MessageContent { Date = DateTime.Now, Id = 1, Text = "Message messasge message"},
                    Incoming = true,
                    IsNew = false
                },
                new PrivateMessage
                {
                    Content = new MessageContent { Date = DateTime.Now, Id = 1, Text = "Message messasge message"},
                    Incoming = false,
                    IsNew = false
                },
                new PrivateMessage
                {
                    Content = new MessageContent { Date = DateTime.Now, Id = 1, Text = "Message messasge message"},
                    Incoming = true,
                    IsNew = false
                },
            };
            //_sender = isender;
            // _handler = ihandler;
            //_webApiClient = webApiClient;

            //PostLogOutCommand = new DelegateCommand(PostLogOutAsync);
            //IsUserSignedInCommand = new DelegateCommand(IsUserSignedInAsync);
            //SearchFriendAsyncCommand = new DelegateCommand(SearchFriendAsync);
            //CreateChatAsyncCommand = new DelegateCommand(CreateChatAsync);
            //SendMessageCommand = new DelegateCommand(SendMessage);
            //InviteToChatAsyncCommand = new DelegateCommand(InviteToChatAsync);
            //LoadAndShowFriendsCommand = new DelegateCommand(LoadAndShowFriends);
            // LoadAndShowChatsCommand = new DelegateCommand(LoadAndShowChats);
            //LoadAndShowPrivateMessagesCommand = new DelegateCommand(LoadAndShowPrivateMessages);
            // LoadAndShowChatMessagesCommand = new DelegateCommand(LoadAndShowChatMessages);
            //FriendSelectedCommand = new DelegateCommand<User>(FriendSelected);
            // ChatSelectedCommand = new DelegateCommand<Chat>(ChatSelected);
            //_handler.InitialInfoRecieved += OnInitialInfoRecieved;
            // _handler.ErrorMessageRecieved += OnErrorMessageRecieved;
            // _handler.UserFoundRecieved += OnUserFoundRecieved;
            // _handler.FriendConfirmedRecieved += OnFriendConfirmedRecieved;
            //_handler.ConnectionStatusRecieved += OnConnectionStatusRecieved;
            // _handler.PrivateMessageRecieved += OnPrivateMessageRecieved;
            // _handler.ChatMessageRecieved += OnChatMessageRecieved;
            //_handler.ChatCreatedRecieved += OnChatCreatedRecieved;
            //_handler.ChatConfirmedRecieved += OnChatConfirmedRecieved;
            //_handler.FriendRequestRecieved += OnFriendRequestRecieved;
            //_handler.ChatRequestRecieved += OnChatRequestRecieved;
            //_handler.PrivateMessagesRecieved += OnPrivateMessagesRecieved;
            // _handler.ChatMessagesRecieved += OnChatMessagesRecieved;
        }

        //private void LoadAndShowPrivateMessages()
        //{
        //    var privateMessageCollection = new ObservableCollection<PrivateMessage>();
        //    privateMessageCollection.AddRange(
        //        _handler._PrivateMessagesManager.GetMessagesByOwner(_handler._ChatGlobals.CurrentFriend));
        //    PrivateMessages = privateMessageCollection;
        //}

        //private void LoadAndShowChatMessages()
        //{
        //    var chatMessagesCollection = new ObservableCollection<ChatMessage>();
        //    chatMessagesCollection.AddRange(
        //        _handler._ChatMessagesManager.GetMessagesByOwner(_handler._ChatGlobals.CurrentChat));
        //    ChatMessages = chatMessagesCollection;
        //}

        //private void LoadAndShowFriends()
        //{
        //    var friendsCollection = new ObservableCollection<User>();
        //    friendsCollection.AddRange(_handler._FriendsManager.GetAll());
        //    Friends = friendsCollection;
        //}

        //private async void FriendSelected(User friendUser)
        //{
        //    _handler._ChatGlobals.SetCurrentFriend(friendUser);
        //    if (!_handler._PrivateMessagesManager.HistoryLoaded(friendUser))
        //    {
        //        await _sender.SendMessageLoadCurrentFriendMessages();
        //    }
        //}

        //private void LoadAndShowChats()
        //{
        //    var chatsCollection = new ObservableCollection<Chat>();
        //    chatsCollection.AddRange(_handler._ChatsManager.GetAll());
        //    Chats = chatsCollection;
        //}

        //private async void ChatSelected(Chat chat)
        //{
        //    _handler._ChatGlobals.SetCurrentChat(_handler._ChatGlobals.ConvertToChatInfo(chat));
        //    if (!_handler._ChatMessagesManager.HistoryLoaded(_handler._ChatGlobals.CurrentChat)) //TODO create container
        //    {
        //        await _sender.SendMessageLoadCurrentChatMessages();
        //    }
        //}

        //private async void SearchFriendAsync()//DONE + Just Post about searching friend
        //{
        //    await _sender.SendMessageSearchFriendAsync(SearchingFriendEmail);
        //}

        //private async void IsUserSignedInAsync()//DONE
        //{
        //    var result = await _webApiClient.UserIsSignedIn();
        //    if (!result)
        //    {
        //        MessageBox.Show("USER IN NOT SIGNED IN!");
        //    }
        //    else
        //    {
        //        MessageBox.Show("Current user logged");
        //    }
        //}

        //private async void PostLogOutAsync()//TODO: Navigate to RegistrationUserContrlol, or Roll back and clean registration entry fields
        //{
        //    await _webApiClient.PostLogout();
        //}

        //private async void InviteToChatAsync()//TODO
        //{
        //    await _sender.SendMessageInviteToChat();
        //}

        //private async void SendMessage()//TODO CHECK IF IT DOESN"T BROKE
        //{
        //    if (_handler._ChatGlobals.CurrentChat != null)
        //    {
        //        await _sender.SendMessageSendChatMessageAsync(_currentUserMessageToSend);
        //    }
        //    else if (_handler._ChatGlobals.CurrentUser != null)
        //    {
        //        await _sender.SendMessageSendPrivateMessageAsync(_currentUserMessageToSend);
        //    }
        //}

        //private async void CreateChatAsync()//DONE
        //{
        //    await _sender.SendMessageCreateChat(ChatNameToCreate);
        //}

        #region Realized EventHandlers

        //private void OnChatMessagesRecieved(object sender, ChatMessagesArguments e)//TODO
        //{
        //    LoadAndShowChatMessages();
        //}

        //private void OnPrivateMessagesRecieved(object sender, PrivateMessagesArguments e)
        //{
        //    LoadAndShowPrivateMessages();
        //    MessageBox.Show("Private messages from User: " + e.UserFrom.Email + " Loaded", e.Messages.ToString(),
        //        MessageBoxButton.OK);
        //}

        //private async void OnChatRequestRecieved(object sender, ChatRequestArguments chatRequestArguments)//DONE
        //{
        //    var messageBoxResult = MessageBox.Show("Chat Invitation Recieved!!!",
        //        "User: " + chatRequestArguments.UserFrom.Email + "Would like to add you to chat: " + chatRequestArguments.Chat.Name,
        //        MessageBoxButton.YesNoCancel);
        //    if (messageBoxResult == MessageBoxResult.Yes)
        //    {
        //        await _sender.SendMessageConfirmChatRequest(true, chatRequestArguments);
        //    }
        //    else if (messageBoxResult == MessageBoxResult.No)
        //    {
        //        await _sender.SendMessageConfirmChatRequest(false, chatRequestArguments);
        //    }
        //}

        //private async void OnFriendRequestRecieved(object sender, FriendRequestArguments friendRequestArguments)//DONE
        //{
        //    var messageBoxResult = MessageBox.Show("Friend Request Received!!!",
        //        "User: " + friendRequestArguments.UserFrom.Email + "Would like to add you to his friend list",
        //        MessageBoxButton.YesNo);
        //    if (messageBoxResult == MessageBoxResult.Yes)
        //    {
        //        await _sender.SendMessageAddFriendAsync(false, true, friendRequestArguments);
        //    }
        //    else if (messageBoxResult == MessageBoxResult.No)
        //    {
        //        await _sender.SendMessageAddFriendAsync(false, false, friendRequestArguments);
        //    }
        //}

        //private void OnChatCreatedRecieved(object sender, ChatCreatedArguments chatCreatedArguments)//DONE
        //{
        //    MessageBox.Show("Your chat was created successfully",
        //        chatCreatedArguments.Chat.Name + " " + chatCreatedArguments.Chat.Id + "Now you can Invite Friends here",
        //        MessageBoxButton.OK);
        //}

        //private void OnChatConfirmedRecieved(object sender, ChatConfirmedArguments chatConfirmedArguments)
        //{
        //    MessageBox.Show(chatConfirmedArguments.Chat.Name,
        //        chatConfirmedArguments.UserFrom.Email + "Confirmed your chat request", MessageBoxButton.OK);
        //}

        //private void OnChatMessageRecieved(object sender, SendChatArguments sendChatArguments)//TODO: add received message to container and show it on the view
        //{
        //    var chatMessagesCollection = new ObservableCollection<ChatMessage>();
        //    chatMessagesCollection.Add(_handler._ChatMessagesManager.CreateMessage((SendChatArguments)sendChatArguments));//метод дозаповняє поля і додає його в контейнер конкретного чату
        //    ChatMessages = chatMessagesCollection;
        //    MessageBox.Show(sendChatArguments.Chat.Name + sendChatArguments.UserFrom.Email, sendChatArguments.Message.Text, MessageBoxButton.OK);
        //}

        //private void OnPrivateMessageRecieved(object sender, SendPrivateArguments sendPrivateArguments)//TODO: add received message to container and show it on the view
        //{
        //    PrivateMessages.Add(_handler._PrivateMessagesManager.CreateMessage((SendPrivateArguments)sendPrivateArguments));//метод дозаповняє поля і додає його в контейнер конкретного френда
        //    MessageBox.Show(sendPrivateArguments.Message.Text, sendPrivateArguments.UserFrom.Email, MessageBoxButton.OK);
        //}

        //private void OnConnectionStatusRecieved(object sender, UserInfo userInfo)//DONE
        //{
        //    String friendStatus = "Online";
        //    if (!userInfo.Online) friendStatus = "Offline";
        //    MessageBox.Show("Friend Status Received", "Your friend " + userInfo.Email + " is " + friendStatus, MessageBoxButton.OK);
        //}

        //private void OnFriendConfirmedRecieved(object sender, FriendConfirmedArguments friendConfirmedArguments)//DONE
        //{
        //    MessageBox.Show("Youe friendship request was accepted", friendConfirmedArguments.UserFrom.Email + "Become your friend!!!", MessageBoxButton.OK);
        //}

        //private async void OnUserFoundRecieved(object sender, UserFoundArguments userFoundArguments)//DONE
        //{
        //    var messageBoxResult = MessageBox.Show("User Found!!!", "Would you like to add him to your Friend List?", MessageBoxButton.YesNo);
        //    if (messageBoxResult == MessageBoxResult.Yes)
        //    {
        //        await _sender.SendMessageAddFriendAsync(true, false, userFoundArguments);
        //    }
        //    else if (messageBoxResult == MessageBoxResult.No)
        //    {
        //        await _sender.SendMessageAddFriendAsync(false, false, userFoundArguments);
        //    }
        //}

        //private void OnErrorMessageRecieved(object sender, string errorMessage)//DONE
        //{
        //    MessageBox.Show("Internal Error Occured", errorMessage, MessageBoxButton.OK);
        //}

        //private void OnInitialInfoRecieved(object sender, InitialInfo initialInfo)//DONE
        //{
        //    LoadAndShowFriends();
        //    LoadAndShowChats();
        //    MessageBox.Show("InitialInfo", "User Authorized", MessageBoxButton.OK);
        //}

        #endregion
    }
}
