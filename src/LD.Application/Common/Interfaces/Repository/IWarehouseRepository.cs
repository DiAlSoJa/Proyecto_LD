using LD.Contracts.DTOs;
using LD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Interfaces.Repository
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        Task<List<DropDownDto>> GetLookup();    
    }
}
