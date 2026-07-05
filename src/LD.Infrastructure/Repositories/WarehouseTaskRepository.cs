using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;
using LD.Domain.Enums;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Repositories;

public class WarehouseTaskRepository : Repository<WarehouseTask>, IWarehouseTaskRepository
{
    public WarehouseTaskRepository(LdProyectDbContext context) : base(context) { }

    public async Task<List<WarehouseTask>> GetTasksAsync(bool soloPendientes, int? warehouseId)
    {
        var query = _context.WarehouseTasks
            .AsNoTracking()
            .Include(x => x.Warehouse)
            .AsQueryable();

        if (soloPendientes)
            query = query.Where(x => x.Status != WarehouseTaskStatus.Completada);

        if (warehouseId.HasValue)
            query = query.Where(x => x.WarehouseId == warehouseId.Value);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<WarehouseTask?> GetTaskByIdAsync(int warehouseTaskId)
    {
        return await _context.WarehouseTasks
            .AsNoTracking()
            .Include(x => x.Warehouse)
            .FirstOrDefaultAsync(x => x.WarehouseTaskId == warehouseTaskId);
    }

    public async Task<WarehouseTask?> GetAssignedTaskForUserAsync(string userId, CancellationToken ct = default)
    {
        return await _context.WarehouseTasks
            .AsNoTracking()
            .Include(x => x.Warehouse)
            .FirstOrDefaultAsync(x =>
                x.AssignedToUserId == userId &&
                x.Status == WarehouseTaskStatus.Asignada, ct);
    }

    public async Task<List<WarehouseTask>> GetPendingUnassignedTasksAsync(CancellationToken ct = default)
    {
        return await _context.WarehouseTasks
            .AsNoTracking()
            .Where(x => x.Status == WarehouseTaskStatus.NoAsignada)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<bool> TryClaimTaskAsync(int warehouseTaskId, string userId, CancellationToken ct = default)
    {
        // ExecuteUpdateAsync con WHERE Status=NoAsignada es atómico a nivel SQL.
        // Si dos procesos intentan reclamar la misma tarea simultáneamente,
        // solo uno obtendrá affectedRows > 0.
        var affected = await _context.WarehouseTasks
            .Where(x =>
                x.WarehouseTaskId == warehouseTaskId &&
                x.Status == WarehouseTaskStatus.NoAsignada)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.Status,           WarehouseTaskStatus.Asignada)
                .SetProperty(x => x.AssignedToUserId, userId)
                .SetProperty(x => x.AssignedAt,       DateTime.UtcNow),
                ct);

        return affected > 0;
    }

    public async Task ReleaseTaskForUserAsync(string userId, CancellationToken ct = default)
    {
        await _context.WarehouseTasks
            .Where(x =>
                x.AssignedToUserId == userId &&
                x.Status == WarehouseTaskStatus.Asignada)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.Status,           WarehouseTaskStatus.NoAsignada)
                .SetProperty(x => x.AssignedToUserId, (string?)null)
                .SetProperty(x => x.AssignedAt,       (DateTime?)null),
                ct);
    }

    public async Task ReleaseAllAssignedTasksAsync(CancellationToken ct = default)
    {
        await _context.WarehouseTasks
            .Where(x => x.Status == WarehouseTaskStatus.Asignada)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.Status,           WarehouseTaskStatus.NoAsignada)
                .SetProperty(x => x.AssignedToUserId, (string?)null)
                .SetProperty(x => x.AssignedAt,       (DateTime?)null),
                ct);
    }

    public async Task ReleaseStaleAssignedTasksAsync(
        IReadOnlyCollection<string> connectedUserIds,
        TimeSpan staleWindow,
        CancellationToken ct = default)
    {
        var staleThreshold = DateTime.UtcNow - staleWindow;

        // Libera si:
        //   a) el usuario ya no está conectado (desconexión sin callback limpio), O
        //   b) lleva más de staleWindow asignada (conexión zombie / usuario bloqueado)
        await _context.WarehouseTasks
            .Where(x =>
                x.Status == WarehouseTaskStatus.Asignada &&
                x.AssignedToUserId != null &&
                (!connectedUserIds.Contains(x.AssignedToUserId!) ||
                 x.AssignedAt < staleThreshold))
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.Status,           WarehouseTaskStatus.NoAsignada)
                .SetProperty(x => x.AssignedToUserId, (string?)null)
                .SetProperty(x => x.AssignedAt,       (DateTime?)null),
                ct);
    }

    public async Task<List<string>> GetUserIdsWithAssignedTaskAsync(
        IReadOnlyCollection<string> userIds,
        CancellationToken ct = default)
    {
        return await _context.WarehouseTasks
            .AsNoTracking()
            .Where(x =>
                x.Status == WarehouseTaskStatus.Asignada &&
                x.AssignedToUserId != null &&
                userIds.Contains(x.AssignedToUserId!))
            .Select(x => x.AssignedToUserId!)
            .Distinct()
            .ToListAsync(ct);
    }

    public async Task<Dictionary<string, HashSet<int>>> GetWarehouseIdsForUsersAsync(
        IReadOnlyCollection<string> userIds,
        CancellationToken ct = default)
    {
        var rows = await _context.UserWarehouses
            .AsNoTracking()
            .Where(x =>
                x.UserId != null &&
                x.WarehouseId.HasValue &&
                userIds.Contains(x.UserId!))
            .Select(x => new { UserId = x.UserId!, WarehouseId = x.WarehouseId!.Value })
            .ToListAsync(ct);

        return rows
            .GroupBy(x => x.UserId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.WarehouseId).ToHashSet());
    }
}
