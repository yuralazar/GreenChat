using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GreenChat.BLL.DTO;
using GreenChat.DAL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GreenChat.BLL.Services
{
    public class SignInManagerDto
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public SignInManagerDto(SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<ClaimsPrincipal> CreateUserPrincipalAsync(UserDto userDto)
        {
            return await _signInManager.CreateUserPrincipalAsync(MapUser(userDto));
        }

        public bool IsSignedIn(ClaimsPrincipal principal)
        {
            return _signInManager.IsSignedIn(principal);
        }

        public async Task<bool> CanSignInAsync(UserDto userDto)
        {
            return await _signInManager.CanSignInAsync(MapUser(userDto));
        }

        public async Task RefreshSignInAsync(UserDto userDto)
        {
            await _signInManager.RefreshSignInAsync(MapUser(userDto));
        }

        public async Task SignInAsync(UserDto userDto, bool isPersistent, string authenticationMethod = null)
        {
            await _signInManager.SignInAsync(MapUser(userDto), isPersistent, authenticationMethod);
        }

        public async Task SignInAsync(UserDto userDto, AuthenticationProperties authenticationProperties,
            string authenticationMethod = null)
        {
            await _signInManager.SignInAsync(MapUser(userDto), authenticationProperties, authenticationMethod);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task ValidateSecurityStampAsync(ClaimsPrincipal principal)
        {
            await _signInManager.ValidateSecurityStampAsync(principal);
        }

        public async Task<SignInResult> PasswordSignInAsync(UserDto userDto, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return await _signInManager.PasswordSignInAsync(MapUser(userDto), password, isPersistent, lockoutOnFailure);
        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return await _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        }

        public async Task<SignInResult> CheckPasswordSignInAsync(UserDto userDto, string password, bool lockoutOnFailure)
        {
            return await _signInManager.CheckPasswordSignInAsync(MapUser(userDto), password, lockoutOnFailure);
        }

        public async Task<bool> IsTwoFactorClientRememberedAsync(UserDto userDto)
        {
            return await _signInManager.IsTwoFactorClientRememberedAsync(MapUser(userDto));
        }

        public async Task RememberTwoFactorClientAsync(UserDto userDto)
        {
            await _signInManager.RememberTwoFactorClientAsync(MapUser(userDto));
        }

        public async Task ForgetTwoFactorClientAsync()
        {
            await _signInManager.ForgetTwoFactorClientAsync();
        }

        public async Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberClient)
        {
            return await _signInManager.TwoFactorSignInAsync(provider, code, isPersistent, rememberClient);
        }

        public async Task<UserDto> GetTwoFactorAuthenticationUserAsync()
        {
            var appUser = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            return MapUserDto(appUser);
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent)
        {
            return await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent);
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
        {
            return await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
        }

        public IEnumerable<AuthenticationDescription> GetExternalAuthenticationSchemes()
        {
            return _signInManager.GetExternalAuthenticationSchemes();
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null)
        {
            return await _signInManager.GetExternalLoginInfoAsync(expectedXsrf);
        }

        public async Task<IdentityResult> UpdateExternalAuthenticationTokensAsync(ExternalLoginInfo externalLogin)
        {
            return await _signInManager.UpdateExternalAuthenticationTokensAsync(externalLogin);
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl,
            string userId = null)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, userId);
        }

        private ApplicationUser MapUser(UserDto userDto)
        {
            return userDto.User ?? _mapper.Map<ApplicationUser>(userDto);
        }

        private UserDto MapUserDto(ApplicationUser user)
        {
            return _mapper.Map<UserDto>(user);
        }
    }
}
