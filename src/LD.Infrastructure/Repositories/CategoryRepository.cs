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
    public class CategoryRepository : ICategoryRepository
    {
        public readonly LdProyectDbContext _context;
        public readonly IMapper _mapper;
        public CategoryRepository(LdProyectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<bool> CreateAsync(Category newModoe)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Category modelToDelete)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Category>> GetAllWithRelationsAsync()
        {
            return await _context.Categories
                .Include(x => x.Client)
                .Include(x => x.Project)
                .ToListAsync();
        }

        public Task<Category?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Category?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DropDownDto>> GetCategoryByClientAsync(int clientId, int projectId)
        {
            return await _context.Categories
              .AsNoTracking()
              .Where(p => p.ClientId == clientId && p.ProjectId == projectId)
              .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
              .ToListAsync();
        }

        public async Task<List<DropDownDto>> GetLookup()
        {
            return await _context.Categories
                .AsNoTracking()
                .ProjectTo<DropDownDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<List<Category>?> GetManyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Category modelToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
