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
    public class LocationRepository : ILocationRepository
    {
        public readonly LdProyectDbContext _context;
        public readonly IMapper _mapper;

        public LocationRepository(LdProyectDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> CreateAsync(Location newModoe)
        {
            throw new NotImplementedException();
        }

        public Task<Location?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DropDownDto>> GetLookup()
        {
            return await _context.Locations
                .AsNoTracking()
                .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<Location>?> GetManyAsync()
        {
            return await _context.Locations
               .Include(x => x.Warehouse)
               .AsNoTracking()
               .ToListAsync();
        }

        public Task<bool> UpdateAsync(Location modelToUpdate)
        {
            throw new NotImplementedException();
        }

        
    }
}
