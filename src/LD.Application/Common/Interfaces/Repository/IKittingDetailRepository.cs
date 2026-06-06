using LD.Contracts.Kitting;

namespace LD.Application.Common.Interfaces.Repository;

public interface IKittingDetailRepository : IRepository<KittingDetailDto>
{
    Task<List<KittingDetailDto>> GetKittingDetailByKittingIdAsync(int kittingId);
}
