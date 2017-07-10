using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GreenChat.Client_WEB.Controllers;
using GreenChat.Client_WEB.Models;
using GreenChat.Client_WEB.Models.AccountViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel;
using Newtonsoft.Json;

namespace GreenChat.Client_WEB.Client
{
    public class WebApiClient
    {
        private HttpClient _client;
        private string _serverStr;
        private CookieContainer _cookieContainer = new CookieContainer();

        public WebApiClient(string serverAddr)
        {
            _serverStr = serverAddr;
        }

        private HttpClient GetHttpClient(HttpContext context)
        {                       
            if (context != null)
            {
                var cookieCollection = new CookieCollection();
                foreach (var cookiePair in context.Request.Cookies)
                {
                    cookieCollection.Add(new Cookie(cookiePair.Key, cookiePair.Value));
                }

                _cookieContainer.Add(new Uri(_serverStr), cookieCollection);             
            }

            return new HttpClient(new HttpClientHandler {CookieContainer = _cookieContainer});
        }

        public async Task<HttpResponseMessage> PostLogin(HttpContext context, LoginViewModel model)
        {           
            var body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Email", model.Email),
                new KeyValuePair<string, string>("Password", model.Password),
                new KeyValuePair<string, string>("RememberMe", model.RememberMe.ToString())
            };

            var content = new FormUrlEncodedContent(body);
            var result = await GetHttpClient(context).PostAsync(new Uri(_serverStr+"/Account/Login"), content);

            return result;
        }

        public async Task<HttpResponseMessage> PostRegister(HttpContext context, RegisterViewModel model)
        {
            var body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Email", model.Email),
                new KeyValuePair<string, string>("Password", model.Password),
                new KeyValuePair<string, string>("ConfirmPassword", model.ConfirmPassword)
            };

            var content = new FormUrlEncodedContent(body);
            var result = await GetHttpClient(context).PostAsync(new Uri(_serverStr + "/Account/Register"), content);

            return result;
        }

        public async Task<bool> UserIsSignedIn(HttpContext context)
        {
            var result = await GetHttpClient(context).GetAsync(new Uri(_serverStr + "/Account/UserIsSignedIn"));
            var fromResponse = await ReadFromResponse(result);
            return Convert.ToBoolean(fromResponse);  
            
        }        

        public async Task<UserDto> GetCurrentUser(HttpContext context)
        {
            var result = await GetHttpClient(context).GetAsync(new Uri(_serverStr + "/Account/UserInfo"));

            var fromResponse = await ReadFromResponse(result);
            var userDto = JsonConvert.DeserializeObject<UserDto>(fromResponse);

            return userDto;
        }

        public async Task<HttpResponseMessage> GetLogin(HttpContext context)
        {
            var result = await GetHttpClient(context).GetAsync(new Uri(_serverStr + "/Account/Login"));
            return result;
        }

        public async Task<HttpResponseMessage> PostLogout(HttpContext context)
        {            
            var result = await GetHttpClient(context).PostAsync(new Uri(_serverStr + "/Account/Logout"), null);           
            return result;
        }

        private static async Task<string> ReadFromResponse(HttpResponseMessage responseMessage)
        {
            var receiveStream = await responseMessage.Content.ReadAsStreamAsync();
            var readStream = new StreamReader(receiveStream, Encoding.UTF8);
            return readStream.ReadToEnd();
        }

        public Cookie GetIdentityCookie()
        {
            var cookies = _cookieContainer.GetCookies(new Uri(_serverStr));
            var identityCookieName = ".AspNetCore.Identity.Application";
            var identityCookie = cookies[identityCookieName];
            return identityCookie;
        }                              
    }
}