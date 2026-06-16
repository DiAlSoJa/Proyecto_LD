using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories;

public class OperationalTaskRepository : Repository<OperationalTask>, IOperationalTaskRepository
{
    public OperationalTaskRepository(LdProyectDbContext context) : base(context)
    {
    }

    public async Task<List<OperationalTask>> GetTasksAsync(bool soloPendientes, int? warehouseId)
    {
        var query = _context.OperationalTasks
            .AsNoTracking()
            .Include(x => x.Warehouse)
            .AsQueryable();

        if (soloPendientes)
            query = query.Where(x => !x.Completed);

        if (warehouseId.HasValue)
            query = query.Where(x => x.WarehouseId == warehouseId.Value);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<OperationalTask?> GetTaskByIdAsync(int operationalTaskId)
    {
        return await _context.OperationalTasks
            .AsNoTracking()
            .Include(x => x.Warehouse)
            .FirstOrDefaultAsync(x => x.OperationalTaskId == operationalTaskId);
    }
}
