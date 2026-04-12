using AutoMapper;
using AutoMapper.QueryableExtensions;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        public readonly LdProyectDbContext _context;
        public readonly IMapper _mapper;

        public LocationRepository(LdProyectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(Location newModel)
        {
            await _context.Locations.AddAsync(newModel);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateRangeAsync(List<Location> locations)
        {
            await _context.Locations.AddRangeAsync(locations);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Location?> GetByIdAsync(int id)
        {
            return await _context.Locations
                .Include(x => x.Warehouse)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.LocationId == id);
        }

        public async Task<Location?> GetByIdAsync(string id)
        {
            return await _context.Locations
                .Include(x => x.Warehouse)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.LocationName == id);
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

        public async Task<List<string>> GetExistingLocationNamesAsync(int warehouseId, List<string> locationNames)
        {
            var normalizedNames = locationNames
                .Select(x => x.Trim().ToUpper())
                .ToList();

            return await _context.Locations
                .AsNoTracking()
                .Where(x => x.WarehouseId == warehouseId &&
                            normalizedNames.Contains(x.LocationName.Trim().ToUpper()))
                .Select(x => x.LocationName)
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Location modelToUpdate)
        {
            _context.Locations.Update(modelToUpdate);
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<bool> DeleteAsync(Location modelToDelete)
        {
            throw new NotImplementedException();
        }
    }
}