using LD.Contracts.DTOs;
using LD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Interfaces.Repository
{
    public interface ILocationRepository : IRepository<Location>
    {
        Task<List<DropDownDto>> GetLookup();
        Task<bool> CreateAsync(Location newModel);
        Task<bool> CreateRangeAsync(List<Location> locations);
        Task<Location?> GetByIdAsync(int id);
        Task<Location?> GetByIdAsync(string id);        
        Task<List<Location>?> GetManyAsync();
        Task<List<string>> GetExistingLocationNamesAsync(int warehouseId, List<string> locationNames);
        Task<bool> UpdateAsync(Location modelToUpdate);
    }
}
