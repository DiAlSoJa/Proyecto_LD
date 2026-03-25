using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs;
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
                .Include(x => x.Client)
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

        public async Task<List<DropDownDto>> GetLookup()
        {
            return await _context.Families
                .AsNoTracking()
                .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<DropDownDto>> GetFamilyByClientAsync(int clientId, int projectId)
        {
            return await _context.Families
              .AsNoTracking()
              .Where(p => p.ClientId == clientId && p.ProjectId == projectId)
              .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
              .ToListAsync();
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
