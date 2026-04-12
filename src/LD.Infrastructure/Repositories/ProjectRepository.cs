using AutoMapper;
using AutoMapper.QueryableExtensions;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs;

using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        public readonly LdProyectDbContext _context;
        public readonly IMapper _mapper;

        public ProjectRepository(LdProyectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(Project newModel)
        {
            try
            {
                if (newModel == null)
                    return false;

                await _context.Projects.AddAsync(newModel);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public Task<bool> DeleteAsync(Project modelToDelete)
        {
            throw new NotImplementedException();
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .AsNoTracking()
                .Include(p => p.ScanConfigurations)
                    .ThenInclude(sc => sc.SystemField)
                .FirstOrDefaultAsync(w => w.ProjectId == id);
        }

        public async Task<Project?> GetByIdAsync(string id)
        {
            return await _context.Projects
                 .AsNoTracking()
                 .Include(p => p.ScanConfigurations)
                    .ThenInclude(sc => sc.SystemField)
                 .FirstOrDefaultAsync(w => w.ProjectId.ToString() == id);
        }

        public async Task<Project?> GetByIdWithConfigsAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.ScanConfigurations)
                    .ThenInclude(sc => sc.SystemField)
                .FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        public async Task<List<DropDownDto>> GetLookup()
        {
            return await _context.Projects
                .AsNoTracking()
                .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<List<Project>?> GetManyAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<DropDownDto>> GetProjectByClientAsync(int clientId)
        {
            return await _context.Projects
               .AsNoTracking()
               .Where(p => p.ClientId == clientId)
               .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
               .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Project modelToUpdate)
        {
            try
            {
                _context.Projects.Update(modelToUpdate);

                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
