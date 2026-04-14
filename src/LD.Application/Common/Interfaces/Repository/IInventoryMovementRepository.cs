using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository
{
    public interface IInventoryMovementRepository : IRepository<InventoryMovement>
    {
        Task<List<InventoryMovement>> GetAllWithRelationsAsync();
    }
}
