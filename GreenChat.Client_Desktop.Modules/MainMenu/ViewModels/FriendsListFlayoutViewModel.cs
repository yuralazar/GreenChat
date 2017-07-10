using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GreenChat.Client_Desktop.Modules.Service.Handlers;
using GreenChat.Client_Desktop.Modules.Service.Models;
using GreenChat.Data.Instances;
using GreenChat.Data.MessageTypes.SendArgs;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Unity;
using PrismMahAppsSample.Infrastructure.Base;
using PrismMahAppsSample.Infrastructure.Constants;
using PrismMahAppsSample.Infrastructure.Interfaces;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ViewModels
{
    public class FriendsListFlayoutViewModel : ViewModelBase
    {
        //private User _selectedFriend;
        //public User SelectedFriend
        //{
        //    get { return _selectedFriend; }
        //    set { SetProperty(ref _selectedFriend, value); }
        //}

        //public ObservableCollection<User> _friends;
        //public ObservableCollection<User> Friends
        //{
        //    get { return _friends; }
        //    set { SetProperty(ref _friends, value); }
        //}

        private UserMess _selectedFriendMess;
        public UserMess SelectedFriendMess
        {
            get { return _selectedFriendMess; }
            set { SetProperty(ref _selectedFriendMess, value); }
        }

        public ObservableCollection<UserMess> _friendsMess;
        public ObservableCollection<UserMess> FriendsMess
        {
            get { return _friendsMess; }
            set { SetProperty(ref _friendsMess, value); }
        }

        public class UserMess
        {
            private String _newMessages = String.Empty;
            public String NewMessages
            {
                get { return _newMessages; }
                set => _newMessages = value;
            }
            public User FriendUser { get; set; }

            public UserMess(User friend, String newMessages)
            {
                NewMessages = newMessages;
                FriendUser = friend;
            }
        }

        private WebSocketsMessageSender _sender;
        private WebSocketsMessageHandler _handler;

        public DelegateCommand SearchFriendAsyncCommand { get; set; }
        //public DelegateCommand FriendSelectedCommand { get; set; }
        public DelegateCommand FriendMessSelectedCommand { get; set; }

        public FriendsListFlayoutViewModel(WebSocketsMessageHandler ihandler, WebSocketsMessageSender isender)
        {
            _sender = isender;
            _handler = ihandler;
            _handler.InitialInfoRecieved += OnInitialInfoRecieved;
            _handler.FriendConfirmedRecieved += OnFriendConfirmedRecieved;
            _handler.FriendRequestRecieved += OnFriendRequestRecieved;
            _handler.ConnectionStatusRecieved += OnConnectionStatusRecieved;
            _handler.UserFoundRecieved += OnUserFoundRecieved;
            _handler.PrivateMessageRecieved += HandlerOnPrivateMessageRecieved;
            //FriendSelectedCommand = new DelegateCommand(FriendSelected);
            SearchFriendAsyncCommand = new DelegateCommand(SearchFriendAsync);
            FriendMessSelectedCommand = new DelegateCommand(FriendMessSelected);
        }

        private async void FriendMessSelected()//TODO Implemet UnmarkNew
        {
            if (SelectedFriendMess?.FriendUser == null)
                return;

            _handler._ChatGlobals.SetCurrentFriend(SelectedFriendMess.FriendUser);
            var historyLoaded = _handler._PrivateMessagesManager.HistoryLoaded(SelectedFriendMess.FriendUser);
            if (!historyLoaded)
            {
                await _sender.SendMessageLoadCurrentFriendMessages();
            }
            else if (_handler._PrivateMessagesManager.HistoryLoaded(SelectedFriendMess.FriendUser))
            {
                _sender.GiveAllMessagesByFriendId(SelectedFriendMess.FriendUser.Id);
            }
            _sender._PrivateMessagesManager.UnmarkNew(_sender._FriendsManager.GetById(SelectedFriendMess.FriendUser.Id));

            var indexOfChengableFriend = FriendsMess.ToList().FindIndex(0, mess => mess.FriendUser.Id == SelectedFriendMess.FriendUser.Id);

            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                //var num = _sender._PrivateMessagesManager.CountNewByOwner(
                //    _sender._FriendsManager.GetById(SelectedFriendMess.FriendUser.Id)).ToString();

                //if((num == "0"))
                    FriendsMess.Insert(indexOfChengableFriend, new UserMess(_sender._FriendsManager.GetById(SelectedFriendMess.FriendUser.Id), String.Empty));

                //else FriendsMess.Insert(indexOfChengableFriend, new UserMess(_sender._FriendsManager.GetById(SelectedFriendMess.FriendUser.Id), num));

                FriendsMess.RemoveAt(indexOfChengableFriend + 1);
            }));
            
            await _sender.SendMessageReadPrivateMessagesArguments();
        }

        private void HandlerOnPrivateMessageRecieved(object sender, SendPrivateArguments sendPrivateArguments)//GOOD
        {
            var indexOfChengableFriend = FriendsMess.ToList().FindIndex(0, mess => mess.FriendUser.Id == sendPrivateArguments.UserFrom.Id);

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                FriendsMess.Insert(indexOfChengableFriend,
                    new UserMess(_sender._FriendsManager.GetById(sendPrivateArguments.UserFrom.Id),
                        _sender._PrivateMessagesManager.CountNewByOwner(
                            _sender._FriendsManager.GetById(sendPrivateArguments.UserFrom.Id)).ToString()));

                FriendsMess.RemoveAt(indexOfChengableFriend + 1);
            }));
        }

        public async void SearchFriendAsync()//GOOD
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService).ShowInputDialogAsync("Searching Friend", "Input email of your friend");
            }));
        }

        private void OnInitialInfoRecieved(object sender, InitialInfo initialInfo)//GOOD
        {
            LoadAndShowFriends();
        }

        public void OnConnectionStatusRecieved(object sender, UserInfo userInfo)//GOOD
        {
            var indexOfChengableFriend = FriendsMess.ToList().FindIndex(0, mess => mess.FriendUser.Id == userInfo.Id);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var num = _sender._PrivateMessagesManager.CountNewByOwner(_sender._FriendsManager.GetById(userInfo.Id)).ToString();

                if(num == "0") FriendsMess.Insert(indexOfChengableFriend, new UserMess(_sender._FriendsManager.GetById(userInfo.Id), String.Empty));

                else FriendsMess.Insert(indexOfChengableFriend, new UserMess(_sender._FriendsManager.GetById(userInfo.Id), num));
                
                FriendsMess.RemoveAt(indexOfChengableFriend + 1);
            }));
        }

        private async void OnFriendConfirmedRecieved(object sender, FriendConfirmedArguments friendConfirmedArguments)//GOOD
        {
            if (friendConfirmedArguments.Confirmed)
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    FriendsMess.Add(new UserMess(_handler._FriendsManager.GetById(friendConfirmedArguments.UserFrom.Id), String.Empty));
                }));

                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService)
                        .ShowMessageAsnyc("FriendShip Alert", "User: " + friendConfirmedArguments.UserFrom.Email + " accepted your friend request");
                }));
            }
            else if (!friendConfirmedArguments.Confirmed)
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService)
                        .ShowMessageAsnyc("FriendShip Alert", "User: " + friendConfirmedArguments.UserFrom.Email + " rejected your friend request");
                }));
            }
        }

        private async void OnFriendRequestRecieved(object sender, FriendRequestArguments friendRequestArguments)//GOOD
        {
            Task<MessageDialogResult> result = null;
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                result = this.Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService).ShowFriendShipRequestAsnyc("Friendship Request recieved", "User: " + friendRequestArguments.UserFrom.Email + " wants to be friends with you!!!", MessageDialogStyle.AffirmativeAndNegative);
            }));
            if (result.Result == MessageDialogResult.Affirmative)
            {
                await _sender.SendMessageAddFriendAsync(false, true, friendRequestArguments);

                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    FriendsMess.Add(new UserMess(_handler._FriendsManager.GetById(friendRequestArguments.UserFrom.Id), String.Empty));
                }));
            }
            else if (result.Result == MessageDialogResult.Negative)
            {
                await _sender.SendMessageAddFriendAsync(false, false, friendRequestArguments);
            }
        }

        #region Serach Friend Methods}

        private async void OnUserFoundRecieved(object sender, UserFoundArguments userFoundArguments)//GOOD
        {
            int numb = 0;
            if (userFoundArguments.User.Email != null)
            {
                Task<MessageDialogResult> result = null;
                if (numb == 0)
                {
                    ++numb;
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        result = this.Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService).ShowAddToFriendsAsnyc("User: " + userFoundArguments.User.Email + " Found!!!", "Would you like to add him to your Friend List?", MessageDialogStyle.AffirmativeAndNegative);
                    }));
                }
                if (result != null && result.Result == MessageDialogResult.Affirmative)
                {
                    await _sender.SendMessageAddFriendAsync(true, false, userFoundArguments);

                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService).ShowMessageAsnyc("You Request was successfuly sent", "Wait for user confirm your friendship request");
                    }));
                }
            }
            else if (userFoundArguments.User.Email == null)
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService).ShowUserNotFound("User: " + userFoundArguments.User.Email + "NOT Found!!!", "Verify that you entered right email address", MessageDialogStyle.Affirmative);
                }));
            }
        }

        #endregion

        #region Helpers

        private async void LoadAndShowFriends()//GOOD
        {
            var friendsCollection = new ObservableCollection<User>();
            friendsCollection.AddRange(_handler._FriendsManager.GetAll());

            FriendsMess = new ObservableCollection<UserMess>();

            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (var friend in friendsCollection)
                {
                    var num = _sender._PrivateMessagesManager.CountNewByOwner(friend).ToString();
                    if(num == "0") FriendsMess.Add(new UserMess(friend, String.Empty));

                    else FriendsMess.Add(new UserMess(friend, num));
                }
            }));
        }

        #endregion
    }
}
