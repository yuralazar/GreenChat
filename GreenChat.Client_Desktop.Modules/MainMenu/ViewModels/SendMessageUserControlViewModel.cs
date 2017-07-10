using Prism.Commands;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GreenChat.Client_Desktop.Modules.Service.Handlers;
using Microsoft.Practices.Unity;
using PrismMahAppsSample.Infrastructure.Base;
using PrismMahAppsSample.Infrastructure.Constants;
using PrismMahAppsSample.Infrastructure.Interfaces;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ViewModels
{
    public class SendMessageUserControlViewModel : ViewModelBase
    {
        private String _currentUserMessageToSend = String.Empty;
        public String CurrentUserMessageToSend
        {
            get { return _currentUserMessageToSend; }
            set { SetProperty(ref _currentUserMessageToSend, value); }
        }

        private Boolean _isButtonSendEnabled = false;
        public Boolean IsButtonSendEnabled
        {
            get { return _isButtonSendEnabled; }
            set { SetProperty(ref _isButtonSendEnabled, value); }
        }

        private WebSocketsMessageSender _sender;
        
        public DelegateCommand SendMessageCommand { get; set; }
        public SendMessageUserControlViewModel(WebSocketsMessageSender isender)//Constructor
        {
            _sender = isender;
            SendMessageCommand = new DelegateCommand(SendMessage);
            this.IntializeCommands();
            _sender._ChatGlobals.CurrentChatSelected += OnUserPickedMessanger;
        }

        private void OnUserPickedMessanger(object sender, bool e)
        {
            IsButtonSendEnabled = true;
            _sender._ChatGlobals.CurrentChatSelected -= OnUserPickedMessanger;
        }


        private async void SendMessage()//TODO CHECK IF IT DOESN"T BROKE
        {
            if (_sender._ChatGlobals.CurrentChat != null)
            {
                await _sender.SendMessageSendChatMessageAsync(_currentUserMessageToSend);
                CurrentUserMessageToSend = String.Empty;
            }
            else if (_sender._ChatGlobals.CurrentFriend != null)
            {
                await _sender.SendMessageSendPrivateMessageAsync(_currentUserMessageToSend);
                CurrentUserMessageToSend = String.Empty;
            }
            CurrentUserMessageToSend = String.Empty;
        }

        #region Commands

        /// <summary>
        /// Initialize commands
        /// </summary>
        private void IntializeCommands()
        {
            this.ShowModuleAPopupCommand = new DelegateCommand(this.ShowModuleAPopup, this.CanShowModuleAPoupup);
            this.ShowModuleAMessageCommand = new DelegateCommand(this.ShowModuleAMessage, this.CanShowModuleAMessage);
        }

        /// <summary>
        /// Show popup
        /// </summary>
        public ICommand ShowModuleAPopupCommand { get; private set; }

        public bool CanShowModuleAPoupup()
        {
            return true;
        }

        public void ShowModuleAPopup()
        {
            this.RegionManager.RequestNavigate(RegionNames.DialogPopupRegion, PopupNames.ModuleAPopup);
        }

        /// <summary>
        /// Show popup
        /// </summary>
        public ICommand ShowModuleAMessageCommand { get; private set; }

        public bool CanShowModuleAMessage()
        {
            return true;
        }

        /// <summary>
        /// Show message
        /// </summary>
        public void ShowModuleAMessage()
        {
            Task<String> friendEmail = null;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService).ShowInputDialogAsync("Searching Friend", "Input email of your friend");
            }));

            if (friendEmail != null)
            {
                var res = friendEmail.Result;
            }


            //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    this.Container.Resolve<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService).ShowMessageAsnyc("Module A Message", "This is a message from Module A");
            //}));
        }

        #endregion Commands
    }
}
