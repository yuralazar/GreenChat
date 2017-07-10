using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GreenChat.Client_Desktop.Modules.Service.Clients;
using GreenChat.Client_Desktop.Modules.Service.Models;

namespace GreenChat.Client_Desktop.Modules.Authrorization.Services
{
    public class AccountWebApiService
    {
        private WebApiClient _webApiClient;

        public AccountWebApiService()
        {
            //_webApiClient = new WebApiClient();
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
                //Console.WriteLine(e);
                //throw;
            }
        }

        //private async Task ExetuteOpeningWebSockets()
        //{
        //    try
        //    {
        //        await _webSocketsMessageHandler.OpenWebSocketConnection();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}
    }
}
