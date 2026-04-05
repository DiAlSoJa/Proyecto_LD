using AutoMapper;
using AutoMapper.QueryableExtensions;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories
{
    public class SystemFieldRepository : ISystemFieldRepository
    {
        private readonly LdProyectDbContext _context;
        private readonly IMapper _mapper;

        public SystemFieldRepository(LdProyectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> CreateAsync(SystemField newModoe)
        {
            throw new NotImplementedException();
        }

        public Task<SystemField?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<SystemField?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DropDownDto>> GetLookup()
        {
            return await _context.SystemFields
                .AsNoTracking()
                .OrderBy(f => f.Order)
                .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<List<SystemField>?> GetManyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(SystemField modelToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
