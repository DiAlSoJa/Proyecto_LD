using LD.Contracts.DTOs;
using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository
{
    public interface ISystemFieldRepository : IRepository<SystemField>
    {
        Task<List<DropDownDto>> GetLookup();
    }
}
