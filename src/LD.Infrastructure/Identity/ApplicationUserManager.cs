using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.DTOs;
using LD.Contracts.DTOs.Auth;
using LD.Contracts.DTOs.User;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.User;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly LdProyectDbContext _context;
        private readonly IMapper _mapper;


        public ApplicationUserManager(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole>  roleMapper,IMapper mapper,LdProyectDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleMapper;
            _context = context;
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

        public async Task<bool> PermissionExists(int permissionId)
        {
            return await _context.Permissions.AnyAsync(p=>p.PermissionId==permissionId);
        }

        public async Task<UserDto?> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                UserName= user.UserName??"sin username",
                //Email = user.Email ?? "sin email"
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
                //Email = user.Email ?? "sin email
            };
        }

        public async Task<UserRequest?> GetUserByIdAsync(string userId)
        {
            return await _userManager.Users.Include(u=>u.UserRoles).ThenInclude(ur=>ur.Role)
                .Where(u=>u.Id== userId)
                .ProjectTo<UserRequest>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

             
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            return await _userManager.Users
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync(); 
        }

        public async Task<bool> CreateUserAsync(UserRequest user)
        {
            var transation = await _context.Database.BeginTransactionAsync();
            try
            {
                var userExists = await GetUserByNameAsync(user.Username);
                if (userExists != null)
                    return false;

                var newUser = new ApplicationUser
                {
                    UserName = user.Username,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    FullName = user.Name
                };

                var result = await _userManager.CreateAsync(newUser, user.Password);

                if (!result.Succeeded)
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                var role = await _roleManager.FindByIdAsync(user.Role);

                if (role is null)
                    throw new Exception("El rol no existe");

                var addRoleResult = await _userManager.AddToRoleAsync(newUser, role.Name);
                if (!addRoleResult.Succeeded)
                    throw new Exception(string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));

    
                await transation.CommitAsync();
                return true;
            }
            catch
            {
                await transation.RollbackAsync();
                throw;
            }
           
        }
        public async Task<bool> UpdateAsync(UserRequest request)
        {
            var transation= await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId ?? "");

                if (user is null)
                    return false;

                // 🔹 Actualizar propiedades
                user.UserName = request.Username;
                user.Email = request.Email;
                user.IsActive = request.IsActive;
                user.FullName = request.Name;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

                if (!string.IsNullOrWhiteSpace(request.Role))
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);

                    if (currentRoles.Any())
                    {
                        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                        if (!removeResult.Succeeded)
                            throw new Exception(string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                    }
                    var role = await _roleManager.FindByIdAsync(request.Role);

                    if (role is null)
                        throw new Exception("El rol no existe");

                    var addRoleResult = await _userManager.AddToRoleAsync(user, role.Name);

                    if (!addRoleResult.Succeeded)
                        throw new Exception(string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                }

                await transation.CommitAsync();
                return true;
            }
            catch
            {
                await transation.RollbackAsync();
                throw;
            }
        }
        public async Task<RoleRequest?> GetRoleByIdAsync(string roleId)
        {
            var role = await _roleManager.Roles
                .Where(r => r.Id == roleId)
                .ProjectTo<RoleRequest>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (role == null)
                throw new Exception($"No se pudo obtener el role con id {roleId}");

            return role;
        }
        public async Task<bool> CreateRoleAsync(RoleRequest roleRequest)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var role = await _roleManager.FindByNameAsync(roleRequest.RoleName);
                if (role is not null) throw new Exception($"El role con nombre {roleRequest.RoleName} ya existe");

                role = new ApplicationRole
                {
                    Name = roleRequest.RoleName
                };
                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

                await _context.RolePermissions.AddRangeAsync(roleRequest.Permissions.Select(p => new RolePermission

                { 
                    RoleId = role.Id, PermissionId = p.PermissionId })
                );

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> UpdateRoleAsync(string roleId, RoleRequest roleRequest)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role is null) throw new Exception($"No se pudo actualizar el role con id {roleId}");

                role.Name = roleRequest.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

                var existing = await _context.RolePermissions
                        .Where(x => x.RoleId == roleId)
                        .Select(x => x.PermissionId)
                        .ToListAsync();

                var toAdd = roleRequest.Permissions.Select(x=>x.PermissionId).Except(existing);
                var toRemove = existing.Except(roleRequest.Permissions.Select(x => x.PermissionId));

                await _context.RolePermissions.AddRangeAsync(
                         toAdd.Select(p => new RolePermission
                         {
                             RoleId = roleId,
                             PermissionId = p
                         })
                     );

                var removeEntities = await _context.RolePermissions
                                            .Where(x => x.RoleId == roleId && toRemove.Contains(x.PermissionId))
                                            .ToListAsync();

                _context.RolePermissions.RemoveRange(removeEntities);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;

            }
            catch (Exception ex) {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<GetMeReponse?> GetMe(string userId)
        {
            var user = await _userManager.Users
                .Include(u=>u.UserRoles)
                .FirstOrDefaultAsync(u=>u.Id== userId);
            if (user == null) throw new Exception($"No se pudo obtener el usuario con id {userId}");

            var userRoleId = user.UserRoles.FirstOrDefault()?.RoleId;
            var rol = await GetRoleByIdAsync(userRoleId);

            var permissionIds = rol?.Permissions.Select(x => x.PermissionId).ToList()?? new List<int>();

            var permissions = await _context.Permissions
                .Include(p=>p.Module)
                .Where(p => permissionIds.Contains(p.PermissionId))
                .ToListAsync();

            var modules = permissions
                .GroupBy(p => p.Module)
                .Select(g => new ModuleAuthorizationDto
                {
                    ModuleId = g.Key.ModuleId,
                    ModuleName = g.Key.ModuleName,
                    Permissions = g.Select(p => new PermissionAuthorizationDto
                    {
                        PermissionName = p.PermissionName,
                        Key = p.Key
                    }).ToList()
                }).ToList();

            var response = new GetMeReponse
            {
                Id = user.Id,
                Name = user.FullName,
                UserName = user.UserName,
                Authorization = new AuthorizationDto
                {
                    RoleName = rol?.RoleName,
                    Modules = modules
                }
            };
            return response;
        
         }

        public async Task<List<DropDownDto>> GetRoleLookupAsync()
        {
            return await _roleManager.Roles
                .AsNoTracking()
                .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<List<RolePermissionDto>> GetRolesWithPermissionsAsync()
        {
            return _roleManager.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .AsNoTracking()
                .ProjectTo<RolePermissionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
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
        //   
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
