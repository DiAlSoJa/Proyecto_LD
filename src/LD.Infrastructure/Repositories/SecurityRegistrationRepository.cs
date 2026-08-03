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
            .Include(r => r.Warehouse)
            .Include(r => r.Photos)
            .ToListAsync();

    public async Task<SecurityRegistration?> GetByIdWithCortinaAsync(int id)
        => await _context.SecurityRegistrations
            .Include(r => r.Cortina)
            .Include(r => r.Warehouse)
            .Include(r => r.Photos)
            .FirstOrDefaultAsync(r => r.SecurityRegistrationId == id);

    public async Task<List<SecurityRegistration>> GetByCreatedAtRangeAsync(DateTime from, DateTime to)
        => await _context.SecurityRegistrations
            .AsNoTracking()
            .Include(r => r.Warehouse)
            .Include(r => r.Photos)
            .Where(x => x.IsActive && x.CreatedAt >= from && x.CreatedAt <= to)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
}
