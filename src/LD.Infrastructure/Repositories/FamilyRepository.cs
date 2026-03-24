using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories
{
    public class FamilyRepository : IFamilyRepository
    {
        public readonly LdProyectDbContext _context;
        public readonly IMapper _mapper;
        public FamilyRepository(LdProyectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<bool> CreateAsync(Family newModoe)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Family>> GetAllWithRelationsAsync()
        {
            return await _context.Families
                .Include(x => x.Project)
                .Include(x => x.Warehouse)
                .ToListAsync();
        }

        public Task<Family?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Family?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Family>?> GetManyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Family modelToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
