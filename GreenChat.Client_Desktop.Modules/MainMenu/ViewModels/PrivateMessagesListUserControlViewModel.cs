using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using GreenChat.Client_Desktop.Modules.Service.Handlers;
using GreenChat.Client_Desktop.Modules.Service.Models;
using GreenChat.Data.Instances;
using GreenChat.Data.MessageTypes.SendArgs;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ViewModels
{
    public class PrivateMessagesListUserControlViewModel : BindableBase
    {
        private WebSocketsMessageHandler _handler;
        private WebSocketsMessageSender _sender;

        private Boolean _isChat = false;
        public Boolean IsChat
        {
            get { return _isChat; }
            set { SetProperty(ref _isChat, value); }
        }

        private ObservableCollection<Message> _commonMessages;
        public ObservableCollection<Message> CommonMessages
        {
            get { return _commonMessages; }
            set { SetProperty(ref _commonMessages, value); }
        }

        public PrivateMessagesListUserControlViewModel(WebSocketsMessageHandler ihandler, WebSocketsMessageSender isender)
        {
            _handler = ihandler;
            _sender = isender;
            _sender.CreatePrivateMessageOutcoming += OnCreatePrivateMessageOutcoming;
            _handler.PrivateMessagesRecieved += OnPrivateMessagesRecieved;
            _handler.PrivateMessageRecieved += OnPrivateMessageRecieved;
            //=================================================================================
            _sender.CreateChatMessageOutcoming += OnCreateChatMessageOutcoming;
            _handler.ChatMessageRecieved += OnChatMessageRecieved;
            _handler.ChatMessagesRecieved += OnChatMessagesRecieved;
            _handler._ChatGlobals.CurrentChatSelected += OnCurrentChatSelected;
            _sender.GotAllChatMessages += OnGotAllChatMessages;
            _sender.GotAllFriendMessages += OnGotAllFriendMessages;
        }



        private void OnGotAllChatMessages(object sender, List<ChatMessage> e)
        {
            CommonMessages.Clear();
            CommonMessages.AddRange(e);
        }

        private void OnGotNewChatMessages(object sender, List<ChatMessage> e)
        {
            CommonMessages.AddRange(e);
        }

        private void OnCurrentChatSelected(object sender, bool e)
        {
            IsChat = e;
        }

        private void OnCreateChatMessageOutcoming(object sender, ChatMessage e)
        {
            if(_sender._ChatGlobals.CurrentChat == null) return;

            if (_handler._ChatGlobals.IsCurrentChat(_handler._ChatGlobals.ConvertToChatInfo(e.Chat)))
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CommonMessages.Add(e);
                }));
            }
        }

        #region ChatMessages

        private void LoadAndShowChatMessages(ChatInfo chatInfo)
        {
            var chatMessagesCollection = new ObservableCollection<Message>();
            
            chatMessagesCollection.AddRange(
                _handler._ChatMessagesManager.GetMessagesByOwner(_handler._ChatsManager.GetById(chatInfo.Id)));

            CommonMessages = chatMessagesCollection;
        }

        private void OnChatMessagesRecieved(object sender, ChatMessagesArguments e)//TODO
        {
            if (_sender._ChatGlobals.CurrentChat == null) return;

            if (_handler._ChatGlobals.IsCurrentChat(e.Chat))
            {
                LoadAndShowChatMessages(e.Chat);
            }
        }

        private void OnChatMessageRecieved(object sender, SendChatArguments sendChatArguments)
        {
            if (_sender._ChatGlobals.CurrentChat == null) return;

            if (_handler._ChatGlobals.IsCurrentChat(sendChatArguments.Chat))
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CommonMessages.Add(_handler._ChatMessagesManager.CreateMessage(sendChatArguments));
                }));
            }
        }

        #endregion

        #region PrivateMessages

        private void LoadAndShowPrivateMessages()
        {
            var privateMessageCollection = new ObservableCollection<Message>();
            privateMessageCollection.AddRange(_handler._PrivateMessagesManager.GetMessagesByOwner(_handler._ChatGlobals.CurrentFriend));

            CommonMessages = privateMessageCollection;
        }

        private void OnGotAllFriendMessages(object sender, List<PrivateMessage> e)
        {
            if(CommonMessages.Count != 0) CommonMessages.Clear();

            CommonMessages.AddRange(e);
        }

        private void OnCreatePrivateMessageOutcoming(object sender, PrivateMessage e)
        {
            if (_handler._ChatGlobals.IsCurrentFriend(_handler._ChatGlobals.ConvertToUserInfo(e.UserTo)))
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CommonMessages.Add(e);
                }));
            }           
        }

        private void OnPrivateMessagesRecieved(object sender, PrivateMessagesArguments e)
        {
            LoadAndShowPrivateMessages();
        }

        private void OnPrivateMessageRecieved(object sender, SendPrivateArguments sendPrivateArguments)
        {
            if (_handler._ChatGlobals.CurrentFriend != null && _handler._ChatGlobals.IsCurrentFriend(sendPrivateArguments.UserFrom))
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CommonMessages.Add(_handler._PrivateMessagesManager.CreateMessage(sendPrivateArguments));
                }));
            }
        }
        #endregion

        #region Helpers

        #endregion
    }
}
