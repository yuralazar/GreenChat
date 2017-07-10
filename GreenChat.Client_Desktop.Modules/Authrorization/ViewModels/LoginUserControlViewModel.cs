using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using GreenChat.Client_Desktop.Modules.MainMenu.Views;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using GreenChat.Client_Desktop.Modules.Service.Models;
using GreenChat.Client_Desktop.Modules.Service.Clients;
using GreenChat.Client_Desktop.Modules.Service.Handlers;
using PrismMahAppsSample.Infrastructure.Base;
using PrismMahAppsSample.Infrastructure.Constants;

namespace GreenChat.Client_Desktop.Modules.Authrorization.ViewModels
{
    public class LoginUserControlViewModel : ViewModelBase, INavigationAware
    {
        IRegionNavigationJournal _journal;
        #region  Properties
        private Boolean _rememberMe = false;

        public Boolean RememberMe
        {
            get { return _rememberMe; }
            set { SetProperty(ref _rememberMe, value); }
        }

        public Visibility _isVisible = Visibility.Hidden;
        public Visibility IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
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

        private string title = string.Empty;
        /// <summary>
        /// Gets or sets the "Title" property
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private string subtitle = string.Empty;
        /// <summary>
        /// Gets or sets the "Subtitle" property
        /// </summary>
        public string Subtitle
        {
            get { return subtitle; }
            set { SetProperty(ref subtitle, value); }
        }
        #endregion Properties

        #region  DelegateCommands, Models, WebApiClient
        private readonly Uri _cookiePath = new Uri(AppDomain.CurrentDomain.BaseDirectory);
        private WebSocketsMessageHandler _webSocketsMessageHandler;
        private UserLoginModel _userLoginModel;
        public UserLoginModel _UserLoginModel
        {
            get => _userLoginModel;
            set => _userLoginModel = value;
        }
        private WebApiClient _webApiClient;
        public DelegateCommand PostLoginAsyncCommand { get; set; }
        public InteractionRequest<INotification> NotificationRequest { get; set; }
        public DelegateCommand GoToRegistrationCommand { get; set; }
        public DelegateCommand GetCookieCommand { get; set; }
        private const String IdentityCookieName = ".AspNetCore.Identity.Application";
        private const String OtherCookieName = ".AspNetCore.Antiforgery.EfEzwGf86wM";

        #endregion

        public DelegateCommand SetCookiesCommand { get; set; }
        private WebSocketsMessageHandler _handler;
        IRegion _region;

        public LoginUserControlViewModel(WebSocketsMessageHandler webSocketsMessageHandler, WebApiClient webApiClient)
        {
            _region = RegionManager.Regions[RegionNames.MainRegion];

            _webSocketsMessageHandler = webSocketsMessageHandler;
          
            _webApiClient = webApiClient;

            PostLoginAsyncCommand = new DelegateCommand(PostLoginAsyncWebApiClient);
            NotificationRequest = new InteractionRequest<INotification>();
            GoToRegistrationCommand = new DelegateCommand(GoToRegistration);
            GetCookieCommand = new DelegateCommand(GetCookie);
        }

        private void GetCookie()
        {
            //IsVisible = Visibility.Visible;
            //var allCookiesArray = Application.GetCookie(_cookiePath);

            //String name = "";
            //String value = "";
            //if (allCookiesArray != null)
            //{
            //    var allCookies = allCookiesArray.Split(';');
            //    for (int i = 0; i < allCookies.Length; i++)
            //    {
            //        allCookies[i] = allCookies[i].Trim();
            //    }

            //    //    if (allCookies[0].StartsWith(OtherCookieName))
            //    //    {
            //    //        if (allCookies[1].TrimStart().StartsWith(IdentityCookieName))
            //    //        {
            //    //            var neededCookie = allCookies[1].TrimStart().Split('=');
            //    //            name = neededCookie[0];
            //    //            value = neededCookie[1];
            //    //        }
            //    //    }
            //    //    else if (allCookies[0].StartsWith(IdentityCookieName))
            //    //    {
            //    //        var neededCookie = allCookies[0].Split('=');
            //    //        name = neededCookie[0];
            //    //        value = neededCookie[1];
            //    //    }
            //    //    cookieContainer.Add(new Cookie(name, value, "/", Ip));
            //    //}
            //    //Application.GetCookie(_cookiePath);

            //    //Application.SetCookie(_cookiePath, ".AspNetCore.Antiforgery.EfEzwGf86wM=00; .AspNetCore.Identity.Application=11; .AspNetCore.Identity.Application=22; .AspNetCore.Antiforgery.EfEzwGf86wM=33");
            //    //Application.SetCookie(_cookiePath, " " + OtherCookieName + "=44");
            //    //Application.SetCookie(_cookiePath, " " + OtherCookieName + "=22");
            //}
        }

        private void GoToRegistration()
        {
            RegionManager.RequestNavigate(RegionNames.MainRegion, UserControlNames.RegistrationUserControl);
        }

        public async void PostLoginAsyncWebApiClient()
        {
           // IsVisible = Visibility.Visible;
            _UserLoginModel = new UserLoginModel()
            {
                Email = this.Email,
                Password = Password.ToString(),
                RememberMe = this.RememberMe
            };

            var responseMessage = await ExecutePostLoginAsync(_UserLoginModel);//Exeption if Server off
            
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

                RegionManager.RequestNavigate(RegionNames.MessagesSenderRegion, "BasicPage1");
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
                    NotificationRequest.Raise(new Notification { Content = "Bad Login Request", Title = "Notification about error" });
                }
            }
            else
            {
                NotificationRequest.Raise(new Notification { Content = "Bad Login Request", Title = "Notification about error" });
            }
        }

        private async Task<HttpResponseMessage> ExecutePostLoginAsync(UserLoginModel userLoginModel)
        {
            try
            {
                return await _webApiClient.PostLogin(userLoginModel);
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
        private bool CanExecute()
        {
            return !String.IsNullOrWhiteSpace(Email) && !String.IsNullOrWhiteSpace(Password);
        }
        #endregion

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _journal = navigationContext.NavigationService.Journal;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }
    }
}
