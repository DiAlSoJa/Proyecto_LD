using LD.Contracts.DTOs;

namespace LD.Application.Common.Interfaces.Repository
{
    public interface ILookupRepository<T> where T : class
    {
        Task<List<DropDownDto>> GetLookup();
    }
}
