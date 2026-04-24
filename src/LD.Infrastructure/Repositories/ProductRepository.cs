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
        public async Task<bool> CreateAsync(Product newModoe)
        {
            try
            {
                if (newModoe == null)
                    return false;

                await _context.items.AddAsync(newModoe);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Product>> GetAllWithRelationsAsync(int? clientId = null, int? projectId = null)
        {
            var query = _context.items
                .Include(x => x.Client)
                .Include(x => x.Project)
                .Include(x=> x.Category)
                .Include(x=>x.Family)
                .Include(x=>x.StorageType)
                .AsQueryable();

            if (clientId.HasValue && projectId.HasValue)
            {
                query = query.Where(x => x.ClientId == clientId.Value && x.ProjectId == projectId.Value);
            }

            return await query.ToListAsync();
        }
        async Task<List<ProductAutocompleteDto>> IProductRepository.GetProductByClientAsync(int clientId, int projectId)
        {
            return await _context.items                
                .Where(p => p.ClientId == clientId && p.ProjectId == projectId)
                .ProjectTo<ProductAutocompleteDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> ExistsByClientProjectAndPartNumberAsync(int clientId, int projectId, string? partNumber, int? excludeProductId = null)
        {
            var normalizedPartNumber = (partNumber ?? string.Empty).Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(normalizedPartNumber))
                return false;

            var query = _context.items.Where(x =>
                x.ClientId == clientId &&
                x.ProjectId == projectId &&
                x.PartNumber != null &&
                x.PartNumber.Trim().ToUpper() == normalizedPartNumber);

            if (excludeProductId.HasValue)
                query = query.Where(x => x.ProductId != excludeProductId.Value);

            return await query.AnyAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.items
                .Include(x => x.Client)
                .Include(x => x.Project)
                .Include(x => x.Category)
                .Include(x => x.Family)
                .Include(x => x.StorageType)
                .FirstOrDefaultAsync(x => x.ProductId == id);
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            if (!int.TryParse(id, out var productId))
                return null;

            return await GetByIdAsync(productId);
        }

        public async Task<List<Product>?> GetManyAsync()
        {
            return await _context.items
                .Include(x => x.Client)
                .Include(x => x.Project)
                .Include(x => x.Category)
                .Include(x => x.Family)
                .Include(x => x.StorageType)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Product modelToUpdate)
        {
            try
            {
                if (modelToUpdate == null)
                    return false;

                _context.items.Update(modelToUpdate);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Product modelToDelete)
        {
            try
            {
                if (modelToDelete == null)
                    return false;

                _context.items.Remove(modelToDelete);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
