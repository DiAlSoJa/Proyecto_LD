using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories;

public class SecurityRegistrationRepository
    : Repository<SecurityRegistration>, ISecurityRegistrationRepository
{
    public SecurityRegistrationRepository(LdProyectDbContext ldProyectDbContext)
        : base(ldProyectDbContext)
    {
    }

    public async Task<List<SecurityRegistration>> GetByCreatedAtRangeAsync(DateTime from, DateTime to)
    {
        return await _context.SecurityRegistrations
            .AsNoTracking()
            .Where(x => x.IsActive && x.CreatedAt >= from && x.CreatedAt <= to)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}
