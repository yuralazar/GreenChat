using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GreenChat.BLL.DTO;
using GreenChat.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace GreenChat.BLL.Services
{
    public class UserManagerDto
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserManagerDto(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public void Dispose()
        {
            _userManager.Dispose();
        }

        public string GetUserName(ClaimsPrincipal principal)
        {
            return _userManager.GetUserName(principal);
        }

        public string GetUserId(ClaimsPrincipal principal)
        {
            return _userManager.GetUserId(principal);
        }

        public async Task<UserDto> GetUserAsync(ClaimsPrincipal principal)
        {
            var appUser = await _userManager.GetUserAsync(principal);
            return MapUserDto(appUser);
        }

        public async Task<string> GenerateConcurrencyStampAsync(UserDto userDto)
        {
            return await _userManager.GenerateConcurrencyStampAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> CreateAsync(UserDto userDto)
        {
            return await _userManager.CreateAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> UpdateAsync(UserDto userDto)
        {
            return await _userManager.UpdateAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> DeleteAsync(UserDto userDto)
        {
            return await _userManager.DeleteAsync(MapAppUser(userDto));
        }

        public async Task<UserDto> FindByIdAsync(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            return MapUserDto(appUser);
        }

        public async Task<UserDto> FindByNameAsync(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);
            return MapUserDto(appUser);
        }

        public async Task<IdentityResult> CreateAsync(UserDto userDto, string password)
        {
            return await _userManager.CreateAsync(MapAppUser(userDto), password);
        }

        public string NormalizeKey(string key)
        {
            return _userManager.NormalizeKey(key);
        }

        public async Task UpdateNormalizedUserNameAsync(UserDto userDto)
        {
            await _userManager.UpdateNormalizedUserNameAsync(MapAppUser(userDto));
        }

        public async Task<string> GetUserNameAsync(UserDto userDto)
        {
            return await _userManager.GetUserNameAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> SetUserNameAsync(UserDto userDto, string userName)
        {
            return await _userManager.SetUserNameAsync(MapAppUser(userDto), userName);
        }

        public async Task<string> GetUserIdAsync(UserDto userDto)
        {
            return await _userManager.GetUserIdAsync(MapAppUser(userDto));
        }

        public async Task<bool> CheckPasswordAsync(UserDto userDto, string password)
        {
            return await _userManager.CheckPasswordAsync(MapAppUser(userDto), password);
        }

        public async Task<bool> HasPasswordAsync(UserDto userDto)
        {
            return await _userManager.HasPasswordAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> AddPasswordAsync(UserDto userDto, string password)
        {
            return await _userManager.AddPasswordAsync(MapAppUser(userDto), password);
        }

        public async Task<IdentityResult> ChangePasswordAsync(UserDto userDto, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(MapAppUser(userDto), currentPassword, newPassword);
        }

        public async Task<IdentityResult> RemovePasswordAsync(UserDto userDto, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _userManager.RemovePasswordAsync(MapAppUser(userDto), cancellationToken);
        }

        public async Task<string> GetSecurityStampAsync(UserDto userDto)
        {
            return await _userManager.GetSecurityStampAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> UpdateSecurityStampAsync(UserDto userDto)
        {
            return await _userManager.UpdateSecurityStampAsync(MapAppUser(userDto));
        }

        public async Task<string> GeneratePasswordResetTokenAsync(UserDto userDto)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> ResetPasswordAsync(UserDto userDto, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(MapAppUser(userDto), token, newPassword);
        }

        public async Task<UserDto> FindByLoginAsync(string loginProvider, string providerKey)
        {
            var appUser = await _userManager.FindByLoginAsync(loginProvider, providerKey);
            return MapUserDto(appUser);
        }

        public async Task<IdentityResult> RemoveLoginAsync(UserDto userDto, string loginProvider, string providerKey)
        {
            return await _userManager.RemoveLoginAsync(MapAppUser(userDto), loginProvider, providerKey);
        }

        public async Task<IdentityResult> AddLoginAsync(UserDto userDto, UserLoginInfo login)
        {
            return await _userManager.AddLoginAsync(MapAppUser(userDto), login);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(UserDto userDto)
        {
            return await _userManager.GetLoginsAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> AddClaimAsync(UserDto userDto, Claim claim)
        {
            return await _userManager.AddClaimAsync(MapAppUser(userDto), claim);
        }

        public async Task<IdentityResult> AddClaimsAsync(UserDto userDto, IEnumerable<Claim> claims)
        {
            return await _userManager.AddClaimsAsync(MapAppUser(userDto), claims);
        }

        public async Task<IdentityResult> ReplaceClaimAsync(UserDto userDto, Claim claim, Claim newClaim)
        {
            return await _userManager.ReplaceClaimAsync(MapAppUser(userDto), claim, newClaim);
        }

        public async Task<IdentityResult> RemoveClaimAsync(UserDto userDto, Claim claim)
        {
            return await _userManager.RemoveClaimAsync(MapAppUser(userDto), claim);
        }

        public async Task<IdentityResult> RemoveClaimsAsync(UserDto userDto, IEnumerable<Claim> claims)
        {
            return await _userManager.RemoveClaimsAsync(MapAppUser(userDto), claims);
        }

        public async Task<IList<Claim>> GetClaimsAsync(UserDto userDto)
        {
            return await _userManager.GetClaimsAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> AddToRoleAsync(UserDto userDto, string role)
        {
            return await _userManager.AddToRoleAsync(MapAppUser(userDto), role);
        }

        public async Task<IdentityResult> AddToRolesAsync(UserDto userDto, IEnumerable<string> roles)
        {
            return await _userManager.AddToRolesAsync(MapAppUser(userDto), roles);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(UserDto userDto, string role)
        {
            return await _userManager.RemoveFromRoleAsync(MapAppUser(userDto), role);
        }

        public async Task<IdentityResult> RemoveFromRolesAsync(UserDto userDto, IEnumerable<string> roles)
        {
            return await _userManager.RemoveFromRolesAsync(MapAppUser(userDto), roles);
        }

        public async Task<IList<string>> GetRolesAsync(UserDto userDto)
        {
            return await _userManager.GetRolesAsync(MapAppUser(userDto));
        }

        public async Task<bool> IsInRoleAsync(UserDto userDto, string role)
        {
            return await _userManager.IsInRoleAsync(MapAppUser(userDto), role);
        }

        public async Task<string> GetEmailAsync(UserDto userDto)
        {
            return await _userManager.GetEmailAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> SetEmailAsync(UserDto userDto, string email)
        {
            return await _userManager.SetEmailAsync(MapAppUser(userDto), email);
        }

        public async Task<UserDto> FindByEmailAsync(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            return MapUserDto(appUser);
        }

        public async Task UpdateNormalizedEmailAsync(UserDto userDto)
        {
            await _userManager.UpdateNormalizedEmailAsync(MapAppUser(userDto));
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(UserDto userDto)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> ConfirmEmailAsync(UserDto userDto, string token)
        {
            return await _userManager.ConfirmEmailAsync(MapAppUser(userDto), token);
        }

        public async Task<bool> IsEmailConfirmedAsync(UserDto userDto)
        {
            return await _userManager.IsEmailConfirmedAsync(MapAppUser(userDto));
        }

        public async Task<string> GenerateChangeEmailTokenAsync(UserDto userDto, string newEmail)
        {
            return await _userManager.GenerateChangeEmailTokenAsync(MapAppUser(userDto), newEmail);
        }

        public async Task<IdentityResult> ChangeEmailAsync(UserDto userDto, string newEmail, string token)
        {
            return await _userManager.ChangeEmailAsync(MapAppUser(userDto), newEmail, token);
        }

        public async Task<string> GetPhoneNumberAsync(UserDto userDto)
        {
            return await _userManager.GetPhoneNumberAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> SetPhoneNumberAsync(UserDto userDto, string phoneNumber)
        {
            return await _userManager.SetPhoneNumberAsync(MapAppUser(userDto), phoneNumber);
        }

        public async Task<IdentityResult> ChangePhoneNumberAsync(UserDto userDto, string phoneNumber, string token)
        {
            return await _userManager.ChangePhoneNumberAsync(MapAppUser(userDto), phoneNumber, token);
        }

        public async Task<bool> IsPhoneNumberConfirmedAsync(UserDto userDto)
        {
            return await _userManager.IsPhoneNumberConfirmedAsync(MapAppUser(userDto));
        }

        public async Task<string> GenerateChangePhoneNumberTokenAsync(UserDto userDto, string phoneNumber)
        {
            return await _userManager.GenerateChangePhoneNumberTokenAsync(MapAppUser(userDto), phoneNumber);
        }

        public async Task<bool> VerifyChangePhoneNumberTokenAsync(UserDto userDto, string token, string phoneNumber)
        {
            return await _userManager.VerifyChangePhoneNumberTokenAsync(MapAppUser(userDto), token, phoneNumber);
        }

        public async Task<bool> VerifyUserTokenAsync(UserDto userDto, string tokenProvider, string purpose, string token)
        {
            return await _userManager.VerifyUserTokenAsync(MapAppUser(userDto), tokenProvider, purpose, token);
        }

        public async Task<string> GenerateUserTokenAsync(UserDto userDto, string tokenProvider, string purpose)
        {
            return await _userManager.GenerateUserTokenAsync(MapAppUser(userDto), tokenProvider, purpose);
        }

        public void RegisterTokenProvider(string providerName, IUserTwoFactorTokenProvider<ApplicationUser> provider)
        {
            _userManager.RegisterTokenProvider(providerName, provider);
        }

        public async Task<IList<string>> GetValidTwoFactorProvidersAsync(UserDto userDto)
        {
            return await _userManager.GetValidTwoFactorProvidersAsync(MapAppUser(userDto));
        }

        public async Task<bool> VerifyTwoFactorTokenAsync(UserDto userDto, string tokenProvider, string token)
        {
            return await _userManager.VerifyTwoFactorTokenAsync(MapAppUser(userDto), tokenProvider, token);
        }

        public async Task<string> GenerateTwoFactorTokenAsync(UserDto userDto, string tokenProvider)
        {
            return await _userManager.GenerateTwoFactorTokenAsync(MapAppUser(userDto), tokenProvider);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(UserDto userDto)
        {
            return await _userManager.GetTwoFactorEnabledAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> SetTwoFactorEnabledAsync(UserDto userDto, bool enabled)
        {
            return await _userManager.SetTwoFactorEnabledAsync(MapAppUser(userDto), enabled);
        }

        public async Task<bool> IsLockedOutAsync(UserDto userDto)
        {
            return await _userManager.IsLockedOutAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> SetLockoutEnabledAsync(UserDto userDto, bool enabled)
        {
            return await _userManager.SetLockoutEnabledAsync(MapAppUser(userDto), enabled);
        }

        public async Task<bool> GetLockoutEnabledAsync(UserDto userDto)
        {
            return await _userManager.GetLockoutEnabledAsync(MapAppUser(userDto));
        }

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(UserDto userDto)
        {
            return await _userManager.GetLockoutEndDateAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> SetLockoutEndDateAsync(UserDto userDto, DateTimeOffset? lockoutEnd)
        {
            return await _userManager.SetLockoutEndDateAsync(MapAppUser(userDto), lockoutEnd);
        }

        public async Task<IdentityResult> AccessFailedAsync(UserDto userDto)
        {
            return await _userManager.AccessFailedAsync(MapAppUser(userDto));
        }

        public async Task<IdentityResult> ResetAccessFailedCountAsync(UserDto userDto)
        {
            return await _userManager.ResetAccessFailedCountAsync(MapAppUser(userDto));
        }

        public async Task<int> GetAccessFailedCountAsync(UserDto userDto)
        {
            return await _userManager.GetAccessFailedCountAsync(MapAppUser(userDto));
        }

        public async Task<IList<UserDto>> GetUsersForClaimAsync(Claim claim)
        {
            var list = await _userManager.GetUsersForClaimAsync(claim);
            return list.Select(MapUserDto).ToList();
        }

        public async Task<IList<UserDto>> GetUsersInRoleAsync(string roleName)
        {
            var list = await _userManager.GetUsersInRoleAsync(roleName);
            return list.Select(MapUserDto).ToList();
        }

        public async Task<string> GetAuthenticationTokenAsync(UserDto userDto, string loginProvider, string tokenName)
        {
            return await _userManager.GetAuthenticationTokenAsync(MapAppUser(userDto), loginProvider, tokenName);
        }

        public async Task<IdentityResult> SetAuthenticationTokenAsync(UserDto userDto, string loginProvider, string tokenName, string tokenValue)
        {
            return await _userManager.SetAuthenticationTokenAsync(MapAppUser(userDto), loginProvider, tokenName, tokenValue);
        }

        public async Task<IdentityResult> RemoveAuthenticationTokenAsync(UserDto userDto, string loginProvider, string tokenName)
        {
            return await _userManager.RemoveAuthenticationTokenAsync(MapAppUser(userDto), loginProvider, tokenName);
        }

        public bool SupportsUserAuthenticationTokens => _userManager.SupportsUserAuthenticationTokens;

        public bool SupportsUserTwoFactor => _userManager.SupportsUserTwoFactor;

        public bool SupportsUserPassword => _userManager.SupportsUserPassword;

        public bool SupportsUserSecurityStamp => _userManager.SupportsUserSecurityStamp;

        public bool SupportsUserRole => _userManager.SupportsUserRole;

        public bool SupportsUserLogin => _userManager.SupportsUserLogin;

        public bool SupportsUserEmail => _userManager.SupportsUserEmail;

        public bool SupportsUserPhoneNumber => _userManager.SupportsUserPhoneNumber;

        public bool SupportsUserClaim => _userManager.SupportsUserClaim;

        public bool SupportsUserLockout => _userManager.SupportsUserLockout;

        public bool SupportsQueryableUsers => _userManager.SupportsQueryableUsers;

        public IQueryable<ApplicationUser> Users => _userManager.Users;

        private ApplicationUser MapAppUser(UserDto userDto)
        {
            return userDto.User ?? _mapper.Map<ApplicationUser>(userDto);
        }

        private UserDto MapUserDto(ApplicationUser user)
        {
            return _mapper.Map<UserDto>(user);
        }
    }
}
