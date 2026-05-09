using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository;

public interface IAvailableInventoryRepository : IRepository<AvailableInventory>
{
    Task<List<AvailableInventory>> GetAllWithRelationsAsync(int? standardId = null);
}
