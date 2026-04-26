using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository;

public interface ISecurityRegistrationRepository : IRepository<SecurityRegistration>
{
    Task<List<SecurityRegistration>> GetManyWithCortinaAsync();
    Task<SecurityRegistration?> GetByIdWithCortinaAsync(int id);
}
