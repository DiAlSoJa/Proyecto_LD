using LD.Contracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Interfaces.Auth
{
    public interface IApplicationUserManager
    {
        //Task<IdentityResponse> RegisterUserAsync(ApplicationUser user);
        Task<UserDto?> FindByEmailAsync(string email);
        Task<UserDto?> GetUserByNameAsync(string userName);
        Task<UserDto?> GetUserByIdAsync(string userId);
        //Task<IList<string>> GetRolesAsync(UserDto user);
        //Task<IList<Claim>> GetClaimsAsync(UserDto user);
        //Task<UserDto> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        //Task<IdentityResponse> AddToRoleAsync(ApplicationUser user, string roleName);
        //Task<IdentityResponse> AddToRolesAsync(ApplicationUser user, List<string> roleNames);
        //Task<IdentityResponse> RemoveFromRoleAsync(ApplicationUser user, string roleName);
        //Task<IdentityResponse> RemoveFromRolesAsync(ApplicationUser user, List<string> roleNames);
        //Task<IdentityResponse> AddClaimsAsync(ApplicationUser user, List<Claim> claims);
        //Task<IdentityResponse> AddClaimAsync(ApplicationUser user, Claim claim);
        //Task<IdentityResponse> RemoveClaimsAsync(ApplicationUser user, List<Claim> claims);
        //Task<IdentityResponse> UpdateAsync(ApplicationUser user);
        //Task<IdentityResponse> HasClaimAsync(ApplicationUser user, Claim claim);
        //Task<IdentityResponse> CheckPasswordAsync(ApplicationUser user, string password);

        //IQueryable<UserDto> Users();

    }
}
