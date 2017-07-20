using System.Threading.Tasks;
using AutoMapper;
using GreenChat.BLL.DTO;
using GreenChat.BLL.Services;
using GreenChat.WebAPI.Models.AccountModels;
using GreenChat.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GreenChat.WebAPI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly string _externalCookieScheme;        
        private readonly IMapper _mapper;

        public AccountController(
            IOptions<IdentityCookieOptions> identityCookieOptions,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory,            
            IMapper mapper,
            UserService userService)
        {
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;            
            _mapper = mapper;
            _userService = userService;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        // GET: /Account/UserIsSignedIn
        [HttpGet]
        [AllowAnonymous]
        public bool UserIsSignedIn()
        {
            return _userService.SignInManager.IsSignedIn(User);
        }

        [HttpGet]
        public async Task<UserDto> UserInfo()
        {
            var userDto = await _userService.UserManager.FindByEmailAsync(User.Identity.Name);
            return userDto;
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);
            return Ok();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model, string returnUrl = null)
        {
            var userEmail = model.Email;
            
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true                

                var result = await _userService.SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User " + userEmail + " logged in.");                    
                    return ResponseHelper.SendStatusCodeSuccess();
                }
                if (result.RequiresTwoFactor)
                {
                    return ResponseHelper.SendStatusCodeTwoFactor();
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User " + userEmail + " account locked out.");
                    return ResponseHelper.SendStatusCodeLocked();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt by user " + userEmail);
                    return ResponseHelper.SendStatusCodeInvalidLogin();
                }
            }
            
            return ResponseHelper.SendStatusCodeBadRequest();
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return ResponseHelper.SendStatusCodeSuccess();                       
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register model)
        {            
            if (ModelState.IsValid)
            {
                var userDto = _mapper.Map<UserDto>(model);
                var result = await _userService.UserManager.CreateAsync(userDto, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userService.UserManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    await _userService.SignInManager.SignInAsync(userDto, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return ResponseHelper.SendStatusCodeSuccess();
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return ResponseHelper.SendStatusCodeBadRequest();
        }

        //
        // POST: /Account/Logout
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return ResponseHelper.SendStatusCodeSuccess();
        }
        
        #region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        #endregion
    }
}
