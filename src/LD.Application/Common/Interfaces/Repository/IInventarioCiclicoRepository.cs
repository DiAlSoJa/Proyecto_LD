using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository;

public interface IInventarioCiclicoRepository : IRepository<CyclicInventory>
{
    Task<List<CyclicInventory>> GetAllWithRelationsAsync(
        DateTime? desde,
        DateTime? hasta,
        string? estatus,
        string? auditorUserId);
    Task<CyclicInventory?> GetByIdWithRelationsAsync(int id);
}
