using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository;

public interface IWarehouseTaskRepository : IRepository<WarehouseTask>
{
    Task<List<WarehouseTask>> GetTasksAsync(bool soloPendientes, int? warehouseId);
    Task<WarehouseTask?> GetTaskByIdAsync(int warehouseTaskId);

    // Retorna la tarea actualmente asignada al usuario, o null si no tiene ninguna.
    Task<WarehouseTask?> GetAssignedTaskForUserAsync(string userId, CancellationToken ct = default);

    // Tareas con Status=NoAsignada, ordenadas por CreatedAt ascendente (FIFO).
    Task<List<WarehouseTask>> GetPendingUnassignedTasksAsync(CancellationToken ct = default);

    // UPDATE WHERE Status=NoAsignada — atómico. Devuelve true si la fila fue reclamada.
    Task<bool> TryClaimTaskAsync(int warehouseTaskId, string userId, CancellationToken ct = default);

    // Libera la tarea asignada a userId (Status NoAsignada, limpia AssignedToUserId/AssignedAt).
    Task ReleaseTaskForUserAsync(string userId, CancellationToken ct = default);

    // Libera TODAS las tareas en estado Asignada. Llamado en startup del worker para limpiar huérfanas.
    Task ReleaseAllAssignedTasksAsync(CancellationToken ct = default);

    // Libera tareas cuyo usuario ya no está en connectedUserIds o lleva más de staleWindow asignada.
    Task ReleaseStaleAssignedTasksAsync(
        IReadOnlyCollection<string> connectedUserIds,
        TimeSpan staleWindow,
        CancellationToken ct = default);

    // Retorna los userIds de connectedUserIds que ya tienen una tarea Asignada en BD.
    Task<List<string>> GetUserIdsWithAssignedTaskAsync(
        IReadOnlyCollection<string> userIds,
        CancellationToken ct = default);

    // Retorna, para cada userId dado, el conjunto de WarehouseId que tiene asignados (UserWarehouse).
    // Usuarios sin almacenes no aparecen en el diccionario.
    Task<Dictionary<string, HashSet<int>>> GetWarehouseIdsForUsersAsync(
        IReadOnlyCollection<string> userIds,
        CancellationToken ct = default);
}
