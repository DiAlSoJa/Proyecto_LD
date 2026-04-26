using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories;

public class SecurityTaskRepository : Repository<SecurityTask>, ISecurityTaskRepository
{
    public SecurityTaskRepository(LdProyectDbContext context) : base(context) { }

    public async Task<List<SecurityTask>> GetManyWithRegistracionAsync()
        => await _context.SecurityTasks
            .AsNoTracking()
            .Include(t => t.SecurityRegistration)
                .ThenInclude(r => r.Cortina)
            .ToListAsync();
}
