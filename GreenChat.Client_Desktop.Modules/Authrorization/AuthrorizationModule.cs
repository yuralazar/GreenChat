using System;
using System.Threading.Tasks;
using GreenChat.Client_Desktop.Modules.Authrorization.Views;
using GreenChat.Client_Desktop.Modules.MainMenu.Views;
using GreenChat.Client_Desktop.Modules.Service.Clients;
using GreenChat.Client_Desktop.Modules.Service.Handlers;
using Microsoft.Practices.Unity;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using Prism.Unity;
using PrismMahAppsSample.Infrastructure.Base;
using PrismMahAppsSample.Infrastructure.Constants;

namespace GreenChat.Client_Desktop.Modules.Authrorization
{
    public class AuthrorizationModule : PrismBaseModule
    {
        private WebApiClient _webApiClient;
        private WebSocketsMessageHandler _webSocketsMessageHandler;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="unityContainer">The Unity container.</param>
        /// <param name="regionManager">The region manager.</param>
        public AuthrorizationModule(IUnityContainer unityContainer, IRegionManager regionManager,
            WebApiClient webApiClient, WebSocketsMessageHandler webSocketsMessageHandler) :
            base(unityContainer, regionManager)
        {
            _webSocketsMessageHandler = webSocketsMessageHandler;
            _webApiClient = webApiClient;
        }

        public override void Initialize()
        {
            UnityContainer.RegisterTypeForNavigation<LoginUserControl>();
            UnityContainer.RegisterTypeForNavigation<RegistrationUserControl>();
            UnityContainer.RegisterTypeForNavigation<LogoutUserControl>();
            UnityContainer.RegisterTypeForNavigation<SendMessageUserControl>();
            UnityContainer.RegisterTypeForNavigation<PrivateMessagesListUserControl>();

            var result = false;//_webApiClient.UserIsSignedInAsync();

            if (!result)
            {
                RegionManager.RequestNavigate(RegionNames.MainRegion, UserControlNames.LoginUserControl);
            }
            else
            {
                RegionManager.RequestNavigate(RegionNames.MainRegion, UserControlNames.LoginUserControl);
                RegionManager.RequestNavigate(RegionNames.MessagesSenderRegion, "BasicPage1");
                //Views
                RegionManager.RequestNavigate(RegionNames.MessagesSenderRegion,
                    UserControlNames.SendMessageUserControl);
                RegionManager.RequestNavigate(RegionNames.TopBarRegion, UserControlNames.LogoutUserControl);
                RegionManager.RequestNavigate(RegionNames.MessagesFlowRegion,
                    UserControlNames.PrivateMessagesListUserControl);
                //TitleBar
                RegionManager.RegisterViewWithRegion(RegionNames.LeftWindowCommandsRegion,
                    typeof(LeftTitlebarCommands));

                // Flyouts
                RegionManager.RegisterViewWithRegion(RegionNames.FlyoutRegion, typeof(FriendsListFlayout));
                RegionManager.RegisterViewWithRegion(RegionNames.FlyoutRegion, typeof(ChatsListFlayout));

                try
                {
                     ExetuteOpeningWebSockets();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            //_regionManager.RequestNavigate(RegionNames.MainRegion, UserControlNames.RegistrationUserControl);
            //Stay on RegistrationPage
        }
        private async void ExetuteOpeningWebSockets()
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
    }
}
