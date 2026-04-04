using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs.Auth;
using LD.Domain.Common;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure.Repositories
{
    public class ModuleRepository
    : IModuleRepository
    {
        protected readonly LdProyectDbContext _context;
        public ModuleRepository(LdProyectDbContext ldProyectDbContext)
        {
            _context = ldProyectDbContext;
        }

        public async Task<List<ModuleAuthorizationDto>> GetModules()
        {
            var modules = await _context.Modules
                .Include(m => m.Permissions)
                .Include(m => m.Children)!
                    .ThenInclude(c => c.Permissions)
                .Where(m => m.ParentModuleId == null)
                .Select(m => new ModuleAuthorizationDto
                {
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName,
                    Permissions = m.Permissions.Select(p => new PermissionAuthorizationDto
                    {
                        PermissionId = p.PermissionId,
                        Key = p.Key,
                        PermissionName = p.PermissionName
                    }).ToList(),
                    SubModules = m.Children.Select(c => new ModuleAuthorizationDto
                    {
                        ModuleId = c.ModuleId,
                        ModuleName = c.ModuleName,
                        Permissions = c.Permissions.Select(p => new PermissionAuthorizationDto
                        {
                            PermissionId = p.PermissionId,
                            Key = p.Key,
                            PermissionName = p.PermissionName
                        }).ToList()
                    }).ToList()
                }).ToListAsync();

            return modules;
        }
    }

}
