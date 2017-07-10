using GreenChat.Client_Desktop.Modules.MainMenu.Views;
using Microsoft.Practices.Unity;
using Prism.Regions;
using Prism.Unity;
using PrismMahAppsSample.Infrastructure.Base;
using PrismMahAppsSample.Infrastructure.Constants;

namespace GreenChat.Client_Desktop.Modules.MainMenu
{
    public class MainMenuModule : PrismBaseModule
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="unityContainer">The Unity container.</param>
        /// <param name="regionManager">The region manager.</param>
        public MainMenuModule(IUnityContainer unityContainer, IRegionManager regionManager) :
            base(unityContainer, regionManager)
        {   // Titlebar
            //regionManager.RegisterViewWithRegion(RegionNames.LeftWindowCommandsRegion, typeof(LeftTitlebarCommands));

            //// Flyouts
            //regionManager.RegisterViewWithRegion(RegionNames.FlyoutRegion, typeof(FriendsListFlayout));
            //regionManager.RegisterViewWithRegion(RegionNames.FlyoutRegion, typeof(ChatsListFlayout));

            // Tiles
           // regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(HomeTiles));
            // Register Views
            Prism.Unity.UnityExtensions.RegisterTypeForNavigation<Views.ModuleAPopup>(unityContainer, PopupNames.ModuleAPopup);
        }

        public override void Initialize()
        {
            UnityContainer.RegisterTypeForNavigation<MainMenuUserControl>();
            UnityContainer.RegisterTypeForNavigation<FriendsListFlayout>();
            UnityContainer.RegisterTypeForNavigation<ChatsListFlayout>();
            UnityContainer.RegisterTypeForNavigation<PrivateMessagesListUserControl>();
            UnityContainer.RegisterTypeForNavigation<TestModernDialog>();
            UnityContainer.RegisterTypeForNavigation<ModuleAPopup>();
            UnityContainer.RegisterTypeForNavigation<ModernDialog1>();
            UnityContainer.RegisterTypeForNavigation<ModernDialog2>();


            //_regionManager.RequestNavigate("SideButtonRegion", "SideBarButtonsControl");
            //_regionManager.RequestNavigate("FriendsListRegion", "FriendsListControl");
            //_regionManager.RequestNavigate("MessageListRegion", "MessageListControl");
        }
    }
}