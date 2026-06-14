using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Domain.Enums;
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
            query = query.Where(x => x.Status != OperationalTaskStatus.Completada);

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

    public async Task<List<OperationalTask>> GetPendingUnassignedTasksAsync(CancellationToken ct = default)
    {
        return await _context.OperationalTasks
            .AsNoTracking()
            .Include(x => x.Warehouse)
            .Where(x => x.Status == OperationalTaskStatus.NoAsignada && x.IsActive)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<bool> TryClaimTaskAsync(int taskId, string userId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        // UPDATE atómico: SQL Server aplica lock de fila, solo un hilo puede ganar la carrera.
        // Si Status ya no es 0 (NoAsignada), rows = 0 → devuelve false → caller intenta siguiente.
        var rows = await _context.Database.ExecuteSqlAsync(
            $"UPDATE OperationalTasks SET Status = 1, AssignedToUserId = {userId}, AssignedAt = {now} WHERE OperationalTaskId = {taskId} AND Status = 0",
            ct);
        return rows > 0;
    }

    public async Task ReleaseTaskForUserAsync(string userId, CancellationToken ct = default)
    {
        await _context.Database.ExecuteSqlAsync(
            $"UPDATE OperationalTasks SET Status = 0, AssignedToUserId = NULL, AssignedAt = NULL WHERE AssignedToUserId = {userId} AND Status = 1",
            ct);
    }

    public async Task<OperationalTask?> GetAssignedTaskForUserAsync(string userId, CancellationToken ct = default)
    {
        return await _context.OperationalTasks
            .AsNoTracking()
            .Include(x => x.Warehouse)
            .FirstOrDefaultAsync(
                x => x.AssignedToUserId == userId && x.Status == OperationalTaskStatus.Asignada,
                ct);
    }

    public async Task<HashSet<string>> GetUserIdsWithAssignedTaskAsync(IEnumerable<string> userIds, CancellationToken ct = default)
    {
        var userIdList = userIds.ToList();
        if (userIdList.Count == 0)
            return [];

        var result = await _context.OperationalTasks
            .AsNoTracking()
            .Where(x => x.Status == OperationalTaskStatus.Asignada
                     && x.AssignedToUserId != null
                     && userIdList.Contains(x.AssignedToUserId))
            .Select(x => x.AssignedToUserId!)
            .Distinct()
            .ToListAsync(ct);

        return [.. result];
    }

    public async Task ReleaseAllAssignedTasksAsync(CancellationToken ct = default)
    {
        await _context.Database.ExecuteSqlAsync(
            $"UPDATE OperationalTasks SET Status = 0, AssignedToUserId = NULL, AssignedAt = NULL WHERE Status = 1",
            ct);
    }

    public async Task ReleaseStaleAssignedTasksAsync(
        IEnumerable<string> connectedUserIds,
        TimeSpan staleness,
        CancellationToken ct = default)
    {
        var cutoff         = DateTime.UtcNow - staleness;
        var connectedList  = connectedUserIds.ToList();

        // Tareas asignadas cuyo usuario ya no está en el tracker Y llevan más tiempo del timeout
        var staleTasks = await _context.OperationalTasks
            .Where(t => t.Status == OperationalTaskStatus.Asignada
                     && t.AssignedAt.HasValue
                     && t.AssignedAt < cutoff
                     && t.AssignedToUserId != null
                     && !connectedList.Contains(t.AssignedToUserId!))
            .ToListAsync(ct);

        if (staleTasks.Count == 0)
            return;

        foreach (var task in staleTasks)
        {
            task.Status         = OperationalTaskStatus.NoAsignada;
            task.AssignedToUserId = null;
            task.AssignedAt     = null;
        }

        await _context.SaveChangesAsync(ct);
    }
}
