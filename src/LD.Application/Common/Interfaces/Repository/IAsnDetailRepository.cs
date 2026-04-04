using LD.Contracts.ASN;

namespace LD.Application.Common.Interfaces.Repository
{
    public interface IAsnDetailRepository : IRepository<AsnDetailDto>
    {
        Task<List<AsnDetailDto>> GetAsnDetailByAsnIdAsync(int asnId);
    }
}