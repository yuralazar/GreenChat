using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using GreenChat.Client_Desktop.Modules.MainMenu.Views;
using GreenChat.Client_Desktop.Modules.Service.Clients;
using GreenChat.Client_Desktop.Modules.Service.Handlers;
using GreenChat.Client_Desktop.Modules.Service.Models;
using GreenChat.Data.MessageTypes.SendArgs;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using PrismMahAppsSample.Infrastructure.Base;
using PrismMahAppsSample.Infrastructure.Constants;

namespace GreenChat.Client_Desktop.Modules.Authrorization.ViewModels
{
    public class RegistrationUserControlViewModel : ViewModelBase
    {
        #region  Properties
        private String _userEmail;

        public String UserEmail
        {
            get { return _userEmail; }
            set { SetProperty(ref _userEmail, value); }
        }

        private string _email = null;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private String _password = null;
        public String Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private String _confirmPassword = null;
        public String ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetProperty(ref _confirmPassword, value); }
        }

        private DateTime? _lastUpdated;
        public DateTime? LastUpdated
        {
            get { return _lastUpdated; }
            set { SetProperty(ref _lastUpdated, value); }
        }
        #endregion Properties



        #region DelegateCommands, Models, WebApiClient

        private WebSocketsMessageHandler _webSocketsMessageHandler;
        private InitialInfo _initialInfo;
        private WebApiClient _webApiClient;
        private UserRegistrationModel _userRegistrationModel;
        public UserRegistrationModel _UserRegistrationModel
        {
            get => _userRegistrationModel;
            set => _userRegistrationModel = value;
        }

        public InteractionRequest<INotification> NotificationRequest { get; set; }
        public DelegateCommand PostRegisterAsyncCommand { get; set; }
        public DelegateCommand GoToLoginCommand { get; set; }

        #endregion


        public RegistrationUserControlViewModel(WebSocketsMessageHandler webSocketsMessageHandler, WebApiClient webApiClient)
        {
            _webSocketsMessageHandler = webSocketsMessageHandler;
            _webApiClient = webApiClient;
            NotificationRequest = new InteractionRequest<INotification>();
            PostRegisterAsyncCommand = new DelegateCommand(PostRegisterAsyncWebApiClient);
            GoToLoginCommand = new DelegateCommand(GoToLogin);
        }

        private void GoToLogin()
        {
            RegionManager.RequestNavigate(RegionNames.MainRegion, UserControlNames.LoginUserControl);
        }

        private async void PostRegisterAsyncWebApiClient()
        {
            if (Password == ConfirmPassword && Password.Length >= 6)
            {
                _UserRegistrationModel = new UserRegistrationModel()
                {
                    Email = this.Email,
                    Password = Password.ToString(),
                    ConfirmPassword = ConfirmPassword.ToString()
                };

                var responseMessage = await ExecutePostRegisterAsync(_UserRegistrationModel);
                if (responseMessage.IsSuccessStatusCode)
                {
                    if (RegionManager.Regions[RegionNames.MainRegion] != null)
                    {
                        List<object> views = new List<object>(RegionManager.Regions[RegionNames.MainRegion].Views);

                        foreach (object view in views)
                        {
                            RegionManager.Regions[RegionNames.MainRegion].Remove(view);
                        }
                    }

                    //Views
                    RegionManager.RequestNavigate(RegionNames.MessagesSenderRegion, UserControlNames.SendMessageUserControl);
                    RegionManager.RequestNavigate(RegionNames.TopBarRegion, UserControlNames.LogoutUserControl);
                    RegionManager.RequestNavigate(RegionNames.MessagesFlowRegion, UserControlNames.PrivateMessagesListUserControl);
                    //TitleBar
                    RegionManager.RegisterViewWithRegion(RegionNames.LeftWindowCommandsRegion, typeof(LeftTitlebarCommands));

                    // Flyouts
                    RegionManager.RegisterViewWithRegion(RegionNames.FlyoutRegion, typeof(FriendsListFlayout));
                    RegionManager.RegisterViewWithRegion(RegionNames.FlyoutRegion, typeof(ChatsListFlayout));

                    try
                    {
                        await ExetuteOpeningWebSockets();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        MessageBox.Show("Opening WebSockets unsuccessful",
                            "Something wrong with closing of Sockets", MessageBoxButton.OK, MessageBoxImage.Error);
                        //NotificationRequest.Raise(new Notification { Content = "Bad Login Request", Title = "Notification about error" });
                    }
                }

            } else if (Password != ConfirmPassword)
            {
                MessageBox.Show("Your Password and ConfirmPasswords aren't same",
                    "Enter Same symbols in these labels", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (Password.Length < 6)
            {
                MessageBox.Show("Your Password is too short",
                    "Enter longer password to be able to Register", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //TODO : Handle why after connection I cant go further in code below 
        }

        private async Task<HttpResponseMessage> ExecutePostRegisterAsync(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                return await _webApiClient.PostRegister(userRegistrationModel);
            }
            catch (Exception e)
            {
                MessageBox.Show("GreenChat", "Internal Error occured", MessageBoxButton.OKCancel);
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
        }

        private async Task ExetuteOpeningWebSockets()
        {
            try
            {
                await _webSocketsMessageHandler.OpenWebSocketConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #region Executability

        #endregion
    }
}
