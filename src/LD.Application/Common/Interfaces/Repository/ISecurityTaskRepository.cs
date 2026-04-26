using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository;

public interface ISecurityTaskRepository : IRepository<SecurityTask>
{
    Task<List<SecurityTask>> GetManyWithRegistracionAsync();
}
