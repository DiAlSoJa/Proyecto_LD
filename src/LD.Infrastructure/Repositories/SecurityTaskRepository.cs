using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories;

public class SecurityTaskRepository : Repository<SecurityTask>, ISecurityTaskRepository
{
    public SecurityTaskRepository(LdProyectDbContext context) : base(context) { }

    public async Task<List<SecurityTask>> GetManyWithRegistracionAsync(int? securityRegistrationId = null)
    {
        var query = _context.SecurityTasks
            .AsNoTracking()
            .Include(t => t.SecurityRegistration)
                .ThenInclude(r => r.Cortina)
            .AsQueryable();

        if (securityRegistrationId.HasValue)
        {
            query = query.Where(t => t.SecurityRegistrationId == securityRegistrationId.Value);
        }

        return await query.ToListAsync();
    }
}
