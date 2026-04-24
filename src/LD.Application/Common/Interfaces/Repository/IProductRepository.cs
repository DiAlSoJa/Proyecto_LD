using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Contracts.DTOs;
using LD.Contracts.Product;
using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetAllWithRelationsAsync(int? clientId = null, int? projectId = null);
        Task<List<ProductAutocompleteDto>> GetProductByClientAsync(int clientId, int projectId);
        Task<bool> ExistsByClientProjectAndPartNumberAsync(int clientId, int projectId, string? partNumber, int? excludeProductId = null);
    }
}
