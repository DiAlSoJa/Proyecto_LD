using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories;

public class SecurityRegistrationRepository : Repository<SecurityRegistration>, ISecurityRegistrationRepository
{
    public SecurityRegistrationRepository(LdProyectDbContext context) : base(context) { }

    public async Task<List<SecurityRegistration>> GetManyWithCortinaAsync()
        => await _context.SecurityRegistrations
            .AsNoTracking()
            .Include(r => r.Cortina)
            .ToListAsync();

    public async Task<SecurityRegistration?> GetByIdWithCortinaAsync(int id)
        => await _context.SecurityRegistrations
            .Include(r => r.Cortina)
            .FirstOrDefaultAsync(r => r.SecurityRegistrationId == id);
}
