using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories;

public class InventarioCiclicoRepository : Repository<CyclicInventory>, IInventarioCiclicoRepository
{
    public InventarioCiclicoRepository(LdProyectDbContext context) : base(context)
    {
    }

    public async Task<List<CyclicInventory>> GetAllWithRelationsAsync(
        DateTime? desde,
        DateTime? hasta,
        string? estatus,
        string? auditorUserId)
    {
        var query = _context.CyclicInventories
            .AsNoTracking()
            .Include(x => x.Warehouse)
            .Include(x => x.Details)
                .ThenInclude(x => x.Location)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(auditorUserId))
        {
            query = query.Where(x => x.AuditorUserId == auditorUserId);
        }

        if (desde.HasValue)
        {
            query = query.Where(x => x.Date.Date >= desde.Value.Date);
        }

        if (hasta.HasValue)
        {
            query = query.Where(x => x.Date.Date <= hasta.Value.Date);
        }

        if (!string.IsNullOrWhiteSpace(estatus))
        {
            query = query.Where(x => x.Status == estatus);
        }

        return await query
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.CyclicInventoryId)
            .ToListAsync();
    }

    public async Task<CyclicInventory?> GetByIdWithRelationsAsync(int id)
    {
        return await _context.CyclicInventories
            .Include(x => x.Warehouse)
            .Include(x => x.Details)
                .ThenInclude(x => x.Location)
            .FirstOrDefaultAsync(x => x.CyclicInventoryId == id);
    }
}
