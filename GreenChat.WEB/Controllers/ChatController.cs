using System.Threading.Tasks;
using GreenChat.Client_WEB.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenChat.Client_WEB.Controllers
{
    public class ChatController : Controller
    {
        private readonly WebApiClient _webApiClient;

        public ChatController(WebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public async Task<IActionResult> Index()
        {            
            var isSignedIn = await _webApiClient.UserIsSignedIn(HttpContext);             
            if (isSignedIn)
            {
                var currentUser = await _webApiClient.GetCurrentUser(HttpContext);

                ViewData["userId"] = currentUser.Id;
                ViewData["userEmail"] = currentUser.Email;

                return View();
            }
            else
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }            
        }
      
    }
}
