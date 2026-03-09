using AutoMapper;
using AutoMapper.QueryableExtensions;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs;
using LD.Contracts.Warehouse;
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
    public class WarehouseRepository : IWarehouseRepository
    {
        public readonly LdProyectDbContext _context;
        public readonly IMapper _mapper;

        public WarehouseRepository(LdProyectDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> CreateAsync(Warehouse newModoe)
        {
            throw new NotImplementedException();
        }

        public async Task<Warehouse?> GetByIdAsync(int id)
        {
            return await _context.Warehouses
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.WarehouseId == id);
        }

        public async Task<List<DropDownDto>> GetLookup()
        {
            return await _context.Warehouses
                .AsNoTracking()
                .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<List<Warehouse>?> GetManyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Warehouse modelToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
