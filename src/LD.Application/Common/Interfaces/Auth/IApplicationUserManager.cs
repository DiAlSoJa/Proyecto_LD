using LD.Contracts.DTOs;
using LD.Contracts.DTOs.User;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
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
        Task<bool> CreateUserAsync(UserRequest user);
        Task<UserDto?> FindByEmailAsync(string email);
        Task<UserDto?> GetUserByNameAsync(string userName);
        Task<UserRequest?> GetUserByIdAsync(string userId);
        Task<GetMeReponse?> GetMe(string userId);

        Task<bool> PermissionExists(int permissionId);
        //Task<IList<string>> GetRolesAsync(UserDto user);
        //Task<IList<Claim>> GetClaimsAsync(UserDto user);
        Task<List<UserDto>> GetUsersAsync();
        Task<List<RolePermissionDto>> GetRolesWithPermissionsAsync();

        Task<RoleRequest?> GetRoleByIdAsync(string roleId);
        Task<List<DropDownDto>> GetRoleLookupAsync();


        //Task<IdentityResponse> AddToRoleAsync(ApplicationUser user, string roleName);
        //Task<IdentityResponse> AddToRolesAsync(ApplicationUser user, List<string> roleNames);
        //Task<IdentityResponse> RemoveFromRoleAsync(ApplicationUser user, string roleName);
        //Task<IdentityResponse> RemoveFromRolesAsync(ApplicationUser user, List<string> roleNames);
        //Task<IdentityResponse> AddClaimsAsync(ApplicationUser user, List<Claim> claims);
        //Task<IdentityResponse> AddClaimAsync(ApplicationUser user, Claim claim);
        //Task<IdentityResponse> RemoveClaimsAsync(ApplicationUser user, List<Claim> claims);
        Task<bool> UpdateAsync(UserRequest user);
        Task<bool> UpdateRoleAsync(string idRole,RoleRequest user);

        //Task<IdentityResponse> HasClaimAsync(ApplicationUser user, Claim claim);
        //Task<IdentityResponse> CheckPasswordAsync(ApplicationUser user, string password);

        //IQueryable<UserDto> Users();

    }
}
