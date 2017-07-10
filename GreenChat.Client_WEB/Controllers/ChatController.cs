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
                return View();
            }
            else
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }            
        }
      
    }
}
