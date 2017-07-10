using Microsoft.Practices.Unity;
using Prism.Unity;
using GreenChat.Client_Desktop.Views;
using System.Windows;
using GreenChat.Client_Desktop.Modules.Authrorization;
using GreenChat.Client_Desktop.Modules.MainMenu;
using Prism.Regions;
using Prism.Modularity;
using GreenChat.Client_Desktop.Modules.Service;
using GreenChat.Client_Desktop.Modules.Service.AutoMapping;
using GreenChat.Client_Desktop.Modules.Service.Handlers;
using GreenChat.Client_Desktop.Modules.Service.Managers;
using AutoMapper;
using GreenChat.Client_Desktop.Modules.Service.Clients;
using GreenChat.Client_Desktop.Properties;
using Prism.Logging;
using PrismMahAppsSample.Infrastructure;
using PrismMahAppsSample.Infrastructure.Constants;
using PrismMahAppsSample.Infrastructure.Interfaces;
using PrismMahAppsSample.Infrastructure.Services;
using WindowNames = PrismMahAppsSample.Infrastructure.Constants.WindowNames;

namespace GreenChat.Client_Desktop
{
    public class Bootstrapper : UnityBootstrapper
    {
        /// <summary>
        /// The shell object
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject CreateShell()
        {   
            Container.RegisterInstance(typeof(Window), WindowNames.MainWindowName, Container.Resolve<MainWindow>(), new ContainerControlledLifetimeManager());
            return Container.Resolve<Window>(WindowNames.MainWindowName);
        }

        /// <summary>
        /// Initialize shell (MainWindow)
        /// </summary>
        protected override void InitializeShell()
        {
            base.InitializeShell();

            // Register views
            var regionManager = this.Container.Resolve<IRegionManager>();
            if (regionManager != null)
            {
                // Add RegistrationUserControl to MainRegion
                //regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(RegistrationUserControl));
            }

            // Register services
            this.RegisterServices();

            //Show Main Window
            Application.Current.MainWindow.Show();
        }

        /// <summary>
        /// Configure the module catalog
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;

            moduleCatalog.AddModule(typeof(AuthrorizationModule));
            moduleCatalog.AddModule(typeof(MainMenuModule));
        }

        /// <summary>
        /// Configure the container
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            // Application commands
            Container.RegisterType<IApplicationCommands, ApplicationCommandsProxy>();
            // Flyout service
            Container.RegisterInstance<IFlyoutService>(Container.Resolve<FlyoutService>());

            //Container.RegisterType(typeof(object), typeof(ViewA), "ViewA");
            //Container.RegisterType(typeof(object), typeof(LoginPage), "LoginPage");
            // Container.RegisterTypeForNavigation<LoginPage>("LoginPage");
        }

        /// <summary>
        /// Register services
        /// </summary>
        private void RegisterServices()//Here we should register our Global Service
        {
            var serverSettings = new ServerSettings(Settings.Default.IP, Settings.Default.Port);
            Container.RegisterInstance(serverSettings, new ContainerControlledLifetimeManager());

            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });

            Container.RegisterInstance(autoMapperConfig.CreateMapper());
            Container.RegisterType<ChatGlobals>(new ContainerControlledLifetimeManager());
            Container.RegisterType<PrivateMessagesManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ChatMessagesManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<FriendsManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ChatsManager>(new ContainerControlledLifetimeManager());

            Container.RegisterType<WebApiClient>(new ContainerControlledLifetimeManager());
            Container.RegisterType< WebSocketsMessageHandler>(new ContainerControlledLifetimeManager());
            Container.RegisterType< WebSocketsMessageSender>(new ContainerControlledLifetimeManager());

            Application.Current.Resources.Add("IoC",this.Container);

            // MessageDisplayServices
            Container.RegisterInstance<IMetroMessageDisplayService>(ServiceNames.MetroMessageDisplayService, Container.Resolve<MetroMessageDisplayService>(), new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Create logger
        /// </summary>
        /// <returns></returns>
        protected override ILoggerFacade CreateLogger()
        {
            //return base.CreateLogger();
            return new Logging.NLogLogger();
        }
    }

    public static class UnityExtensions
    {
        public static void RegisterTypeForNavigation<T>(this IUnityContainer container, string name)
        {
            container.RegisterType(typeof(object), typeof(T), name);
        }
    }

}
