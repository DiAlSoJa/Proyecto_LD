using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Contracts.DTOs;
using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<Project?> GetByIdWithConfigsAsync(int id);
        
        Task<List<DropDownDto>> GetLookup();
        Task<List<DropDownDto>> GetProjectByClientAsync(int clientId);
        Task<List<UserProjectClientDto>> GetProjectClientsByUserWarehousesAsync(string userId);
    }
}
