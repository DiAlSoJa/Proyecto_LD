using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository;

public interface IOperationalTaskRepository : IRepository<OperationalTask>
{
    Task<List<OperationalTask>> GetTasksAsync(bool soloPendientes, int? warehouseId);
    Task<OperationalTask?> GetTaskByIdAsync(int operationalTaskId);

    // Devuelve solo tareas con Status = NoAsignada (candidatas para asignar)
    Task<List<OperationalTask>> GetPendingUnassignedTasksAsync(CancellationToken ct = default);

    // Asignación atómica: UPDATE WHERE Status=NoAsignada → devuelve true si se logró
    Task<bool> TryClaimTaskAsync(int taskId, string userId, CancellationToken ct = default);

    // Libera la tarea actualmente asignada a ese usuario (Status → NoAsignada)
    Task ReleaseTaskForUserAsync(string userId, CancellationToken ct = default);

    // Tarea asignada al usuario autenticado (para my-assigned endpoint y logout check)
    Task<OperationalTask?> GetAssignedTaskForUserAsync(string userId, CancellationToken ct = default);

    // De una lista de userIds, devuelve los que ya tienen una tarea Asignada
    Task<HashSet<string>> GetUserIdsWithAssignedTaskAsync(IEnumerable<string> userIds, CancellationToken ct = default);

    // Libera todas las tareas en estado Asignada (startup cleanup — instancia única)
    Task ReleaseAllAssignedTasksAsync(CancellationToken ct = default);

    // Libera tareas huérfanas: Asignada + usuario no conectado + tiempo excedido
    Task ReleaseStaleAssignedTasksAsync(IEnumerable<string> connectedUserIds, TimeSpan staleness, CancellationToken ct = default);
}
