using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository;

public interface IKittingRepository : IRepository<Kitting>
{
    Task<List<Kitting>> GetKittingByClientAsync(int clientId, int projectId);
    Task<bool> CreateWithSequenceAsync(Kitting entity);
    Task<List<Kitting>> GetAllWithRelationsAsync();
}
