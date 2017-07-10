using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using GreenChat.Client_WEB.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using GreenChat.Client_WEB.Models;
using GreenChat.Client_WEB.Models.AccountViewModels;
using GreenChat.Client_WEB.Services;
using Microsoft.AspNetCore.Http;

namespace GreenChat.Client_WEB.Controllers
{
    public class AccountController : Controller
    {        
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;        
        private readonly IMapper _mapper;
        private readonly WebApiClient _webApiClient;

        public AccountController(
            IOptions<IdentityCookieOptions> identityCookieOptions,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory,                     
            IMapper mapper,
            WebApiClient webApiClient)
        {            
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;                     
            _mapper = mapper;
            _webApiClient = webApiClient;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        public async Task<bool> UserIsSignedIn()
        {
            var isSignedIn = await _webApiClient.UserIsSignedIn(HttpContext);
            return isSignedIn;
        }

        //
        // GET: /Account/Login
        [HttpGet]        
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            var result = await _webApiClient.GetLogin(HttpContext);
                        
            ViewData["ReturnUrl"] = returnUrl;
            return View();            
        }    

        //
        // POST: /Account/Login
        [HttpPost]        
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            var userEmail = model.Email;

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var responseMessage = await _webApiClient.PostLogin(HttpContext, model);                

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    _logger.LogInformation(1, "User " + userEmail + " logged in.");
                    AddIdentityCookieToResponse();                    
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt by user " + userEmail);
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        
        private void AddIdentityCookieToResponse()
        {
            var cookie = _webApiClient.GetIdentityCookie();

            if (cookie == null)
            {
                _logger.LogWarning("Identity cookie is empty!");
                return;
            }

            var cookieOptions = new CookieOptions
            {
                //Domain = cookie.Domain,               
                HttpOnly = cookie.HttpOnly,
                Path = cookie.Path,
                Secure = cookie.Secure
            };

            if (cookie.Expires != DateTime.MinValue)
                cookieOptions.Expires = cookieOptions.Expires;

            Response.Cookies.Append(cookie.Name, cookie.Value, cookieOptions);
            //Response.Headers.Add("P3P", "CP=\"CAO IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
            //Response.Headers.Add("Cache-Control", "no-cache");
            //Response.Headers.Add("Pragma", "no-cache");
            //Response.Headers.Add("Expires", "-1");
        }

        private void RemoveIdentityCookie()
        {

            var cookieOptions = new CookieOptions
            {                
                HttpOnly = true,
                Path = "/"                
            };
            cookieOptions.Expires = DateTime.Now.AddDays(-1);

            Response.Cookies.Append(".AspNetCore.Identity.Application", "", cookieOptions);
        }

        //
        // GET: /Account/Register
        [HttpGet]        
        public IActionResult Register(string returnUrl = null)
        {            
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]        
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _webApiClient.PostRegister(HttpContext, model);
                
                if (result.StatusCode == HttpStatusCode.OK)
                {                   
                    _logger.LogInformation(3, "User created a new account with password.");
                    AddIdentityCookieToResponse();
                    return RedirectToLocal(returnUrl);
                }                
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Logout
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _webApiClient.PostLogout(HttpContext);
            RemoveIdentityCookie();
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }       

        #region Helpers

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }
        //}

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(ChatController.Index), "Chat");
            }
        }
        #endregion
    }
}
