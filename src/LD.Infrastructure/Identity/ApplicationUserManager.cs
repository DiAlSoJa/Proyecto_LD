using LD.Application.Common.Interfaces.Auth;
using LD.Contracts.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure
{
    public class ApplicationUserManager : IApplicationUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserManager(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }

        //public async Task<IdentityResponse> RegisterUserAsync(ApplicationUser user)
        //{
        //    if (user.Email != null && await _userManager.FindByEmailAsync(user.Email) != null)
        //    {
        //        return IdentityResponse.Fail("Email already exists! Please try a different one!");
        //    }
        //    user.UserName ??= user.Email;
        //    if (user.UserName != null && await _userManager.FindByNameAsync(user.UserName) != null)
        //    {
        //        return IdentityResponse.Fail("User Name already exists! Please try a different one");
        //    }
        //    var rs = await _userManager.CreateAsync(user);
        //    return rs.ToIdentityResponse();
        //}

        public async Task<UserDto?> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                UserName= user.UserName??"sin username",
                Email = user.Email ?? "sin email"
            };
        }

        public async Task<UserDto?> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? "sin username",
                Email = user.Email ?? "sin email"
            };
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? "sin username",
                Email = user.Email ?? "sin email"
            };
        }

        //public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        //{
        //    return await _userManager.GetRolesAsync(user);
        //}

        //public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        //{
        //    return await _userManager.GetClaimsAsync(user);
        //}

        //public async Task<UserDto> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        //{
        //    return await _userManager.GetUserAsync(claimsPrincipal);
        //}



        //public async Task<IdentityResponse> AddToRoleAsync(ApplicationUser user, string roleName)
        //{
        //    var rs = await _userManager.AddToRoleAsync(user, roleName);
        //    return rs.ToIdentityResponse();
        //}

        //public async Task<IdentityResponse> AddToRolesAsync(ApplicationUser user, List<string> roleNames)
        //{
        //    var rs = await _userManager.AddToRolesAsync(user, roleNames);
        //    return rs.ToIdentityResponse();
        //}

        //public async Task<IdentityResponse> RemoveFromRoleAsync(ApplicationUser user, string roleName)
        //{
        //    if (roleName == DefaultApplicationRoles.SuperAdmin)
        //        return IdentityResponse.Fail("Can not to delete superAdmin role");

        //    var rs = await _userManager.RemoveFromRoleAsync(user, roleName);
        //    return rs.ToIdentityResponse();
        //}

        //public async Task<IdentityResponse> RemoveFromRolesAsync(ApplicationUser user, List<string> roleNames)
        //{
        //    roleNames = roleNames.Where(x => x != DefaultApplicationRoles.SuperAdmin.ToString()).ToList();
        //    var rs = await _userManager.RemoveFromRolesAsync(user, roleNames);
        //    return rs.ToIdentityResponse();
        //}

        //public async Task<IdentityResponse> AddClaimsAsync(ApplicationUser user, List<Claim> claims)
        //{
        //    var rs = await _userManager.AddClaimsAsync(user, claims);
        //    return rs.ToIdentityResponse();
        //}

        //public async Task<IdentityResponse> AddClaimAsync(ApplicationUser user, Claim claim)
        //{
        //    var rs = await _userManager.AddClaimAsync(user, claim);
        //    return rs.ToIdentityResponse();
        //}

        //public async Task<IdentityResponse> RemoveClaimsAsync(ApplicationUser user, List<Claim> claims)
        //{
        //    var rs = await _userManager.RemoveClaimsAsync(user, claims);
        //    return rs.ToIdentityResponse();
        //}

        //public async Task<IdentityResponse> UpdateAsync(ApplicationUser user)
        //{
        //    var rs = await _userManager.UpdateAsync(user);
        //    return rs.ToIdentityResponse();
        //}

        //public async Task<IdentityResponse> HasClaimAsync(ApplicationUser user, Claim claim)
        //{
        //    var claims = await _userManager.GetClaimsAsync(user);
        //    return claims.Any(x => x.Type == claim.Type && x.Value == claim.Value)
        //        ? IdentityResponse.Success("Claim Exists")
        //        : IdentityResponse.Fail("Claim does not exist");
        //}

        //public IQueryable<UserDto> Users()
        //{
        //    return _userManager.Users;
        //    if (user == null) return null;
        //    return new UserDto
        //    {
        //        Id = user.Id,
        //        UserName = user.UserName ?? "sin username",
        //        Email = user.Email ?? "sin email"
        //    };
        //}

        //public async Task<IdentityResponse> CheckPasswordAsync(ApplicationUser user, string password)
        //{
        //    var rs = await _userManager.CheckPasswordAsync(user, password);
        //    return rs ? IdentityResponse.Success("Password is correct") : IdentityResponse.Fail("Password is not correct");
        //}
    }
}
