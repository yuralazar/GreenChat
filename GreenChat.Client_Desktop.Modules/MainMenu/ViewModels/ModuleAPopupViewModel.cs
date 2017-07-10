using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GreenChat.Client_Desktop.Modules.Service.Handlers;
using GreenChat.Client_Desktop.Modules.Service.Models;
using GreenChat.Data.MessageTypes.SendArgs;
using PrismMahAppsSample.Infrastructure.Base;
using PrismMahAppsSample.Infrastructure.Constants;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ViewModels
{
    public class ModuleAPopupViewModel : ViewModelBase
    {
        private Chat _selectedChat;
        public Chat SelectedChat
        {
            get { return _selectedChat; }
            set { SetProperty(ref _selectedChat, value); }
        }

        private ObservableCollection<Chat> _chats;
        public ObservableCollection<Chat> Chats
        {
            get { return _chats; }
            set { SetProperty(ref _chats, value); }
        }

        public ObservableCollection<User> _friends;
        public ObservableCollection<User> Friends
        {
            get { return _friends; }
            set { SetProperty(ref _friends, value); }
        }

        private WebSocketsMessageSender _sender;
        private WebSocketsMessageHandler _handler;
        public DelegateCommand ChatSelectedCommand { get; set; }

        public class UserCheck
        {
            public Boolean IsChecked { get; set; }
            public String Email { get; set; }
            public User FriendUser { get; set; }

            public UserCheck(User fr)
            {
                IsChecked = false;
                FriendUser = fr;
                Email = fr.Email;
            }
        }

        private List<UserCheck> _userCheckList;
        public List<UserCheck> UserCheckList
        {
            get { return _userCheckList; }
            set { SetProperty(ref _userCheckList, value); }
        }

        public DelegateCommand SelectionChangedCommand { get; set; }
        public DelegateCommand InviteSelectedFriendsToChatAsyncCommand{ get; set; }

        public ModuleAPopupViewModel(WebSocketsMessageHandler ihandler, WebSocketsMessageSender isender)
        {
            SelectionChangedCommand = new DelegateCommand(ChatSelected);
            InviteSelectedFriendsToChatAsyncCommand = new DelegateCommand(InviteSelectedFriendsToChatAsync);
            _sender = isender;
            _handler = ihandler;

            var chatsCollection = new ObservableCollection<Chat>();
            chatsCollection.AddRange(_handler._ChatsManager.GetAll());
            Chats = chatsCollection;

            var friendsCollection = new ObservableCollection<User>();
            friendsCollection.AddRange(_handler._FriendsManager.GetAll());
            Friends = friendsCollection;
        }

        private void ChatSelected()//DOONE
        {
            UserCheckList = new List<UserCheck>();
            var c = _handler._ChatsManager.GetById(SelectedChat.Id);

            foreach (var friend in _handler._FriendsManager.GetAll())
            {
                if(!SelectedChat.Users.Exists(f => f == friend))
                    UserCheckList.Add(new UserCheck(friend));
            }
        }

        private async void InviteSelectedFriendsToChatAsync()
        {
            var friendsToInvite = (from intem in UserCheckList
                where intem.IsChecked == true
                select intem.FriendUser).ToList();

            await _sender.SendMessageInviteToChat(friendsToInvite);

            //await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    RegionManager.Regions[RegionNames.DialogPopupRegion].Remove(PopupNames.ModuleAPopup);
            //    //foreach (var region in RegionManager.Regions)
            //    //{

            //    //}
            //    //RegionManager.RequestNavigate(RegionNames.DialogPopupRegion, PopupNames.ModuleAPopup);
            //}));
        }
    }
}
