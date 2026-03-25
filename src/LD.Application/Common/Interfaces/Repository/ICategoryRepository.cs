using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Contracts.DTOs;
using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository
{
    public  interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> GetAllWithRelationsAsync();
        Task<List<DropDownDto>> GetLookup();
        Task<List<DropDownDto>> GetCategoryByClientAsync(int clientId, int projectId);
    }
}
