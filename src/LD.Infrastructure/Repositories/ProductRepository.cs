using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs;
using LD.Contracts.Product;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public readonly LdProyectDbContext _context;
        public readonly IMapper _mapper;
        public ProductRepository(LdProyectDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<bool> CreateAsync(Product newModoe)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetAllWithRelationsAsync()
        {
            return await _context.items
                .Include(x => x.Client)
                .Include(x => x.Project)
                .Include(x=> x.Category)
                .Include(x=>x.Family)
                .Include(x=>x.StorageType)
                .ToListAsync();
        }
        async Task<List<ProductAutocompleteDto>> IProductRepository.GetProductByClientAsync(int clientId, int projectId)
        {
            return await _context.items                
                .Where(p => p.ClientId == clientId && p.ProjectId == projectId)
                .ProjectTo<ProductAutocompleteDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>?> GetManyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Product modelToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
