using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using GreenChat.Client_Desktop.Modules.Service.Clients;
using GreenChat.Client_Desktop.Modules.Service.Handlers;
using Prism.Regions;
using PrismMahAppsSample.Infrastructure.Base;
using PrismMahAppsSample.Infrastructure.Constants;

namespace GreenChat.Client_Desktop.Modules.Authrorization.ViewModels
{
    public class LogoutUserControlViewModel : ViewModelBase
    {
        private WebSocketsMessageHandler _handler;
        private WebApiClient _webApiClient;
        public DelegateCommand PostLogoutAsyncCommand { get; set; }
        public DelegateCommand IsUserSignedInAsyncCommand { get; set; }

        public LogoutUserControlViewModel(WebApiClient webApiClient, WebSocketsMessageHandler handler)
        {
            _handler = handler;
            _webApiClient = webApiClient;
            PostLogoutAsyncCommand = new DelegateCommand(PostLogoutAsync);
            IsUserSignedInAsyncCommand = new DelegateCommand(IsUserSignedInAsync);
        }

        private async void PostLogoutAsync()
        {
            await ExecuteCloseWebSocketConnection();
            var responseMessage = await ExecutePostLogoutAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                ClearManagersAndGlobals();

                foreach (var region in RegionManager.Regions)
                {
                    if (region != null)
                    {
                        List<object> views = new List<object>(RegionManager.Regions[region.Name].Views);

                        foreach (object view in views)
                        {

                            RegionManager.Regions[region.Name].Remove(view);
                        }
                    }
                }
                RegionManager.RequestNavigate(RegionNames.MainRegion, UserControlNames.LoginUserControl);
 
            }
            else
            {
                MessageBox.Show("Logout unsuccessful", "Some String Caption");
            }
        }

        private void ClearManagersAndGlobals()
        {
            _handler._PrivateMessagesManager.Clear();
            _handler._ChatMessagesManager.Clear();
            _handler._FriendsManager.Clear();
            _handler._ChatsManager.Clear();

            _handler._ChatGlobals.Clear();
        }

        private async Task ExecuteCloseWebSocketConnection()
        {
            Task task;
            try
            {
                await _handler.CloseWebSocketConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
                MessageBox.Show("GreenChat", "Internal Error occured", MessageBoxButton.OKCancel);
            }
        }

        private async Task<HttpResponseMessage> ExecutePostLogoutAsync()
        {
            var responseMessage = new HttpResponseMessage();
            try
            {
                 responseMessage = await _webApiClient.PostLogout();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
                MessageBox.Show("GreenChat", "Internal Error occured", MessageBoxButton.OKCancel);
            }
            return responseMessage;
        }

        private async void IsUserSignedInAsync()//DONE
        {
            var result = await _webApiClient.UserIsSignedInAsync();
            if (!result)
            {
                MessageBox.Show("USER IN NOT SIGNED IN!");
            }
            else
            {
                MessageBox.Show("Current user logged");
            }
        }
    }
}
