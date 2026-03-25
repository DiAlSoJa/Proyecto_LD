using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        public readonly LdProyectDbContext _context;
        public readonly IMapper _mapper;
        public UnitRepository(LdProyectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<bool> CreateAsync(Unit newModoe)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateAsync(Units newModoe)
        {
            throw new NotImplementedException();
        }

        public Task<Unit?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Unit?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DropDownDto>> GetLookup()
        {
            return await _context.Units
                .AsNoTracking()
                .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<List<Unit>?> GetManyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Unit modelToUpdate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Units modelToUpdate)
        {
            throw new NotImplementedException();
        }

        Task<Units?> IRepository<Units>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<Units?> IRepository<Units>.GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        Task<List<Units>?> IRepository<Units>.GetManyAsync()
        {
            throw new NotImplementedException();
        }
    }
}
