using Microsoft.AspNetCore.Mvc;

namespace GreenChat.WebAPI.Controllers
{
    public static class ResponseHelper
    {
        public static IActionResult SendStatusCode(int code)
        {
            return new StatusCodeResult(code);
        }        

        public static IActionResult SendStatusCodeSuccess()
        {
            return SendStatusCode(200);
        }

        public static IActionResult SendStatusCodeTwoFactor()
        {
            return SendStatusCode(203);
        }

        public static IActionResult SendStatusCodeLocked()
        {
            return SendStatusCode(417);
        }

        public static IActionResult SendStatusCodeInvalidLogin()
        {
            return SendStatusCode(401);
        }

        public static IActionResult SendStatusCodeBadRequest()
        {
            return SendStatusCode(400);
        }
    }
}
