using LD.Application.Common.Interfaces;
using System.Collections.Concurrent;

namespace LD.Api.Services;

/// <summary>
/// Singleton que rastrea qué usuarios están conectados al hub y qué tarea tienen asignada.
/// Implementa ITaskAssignmentTracker para que los handlers de LD.Application puedan
/// actualizar asignaciones sin saber nada de SignalR.
/// </summary>
public sealed class ConnectedUsersTracker : ITaskAssignmentTracker
{
    // userId → taskId asignado (null = conectado pero sin tarea)
    private readonly ConcurrentDictionary<string, int?> _state = new();

    // ── Lifecycle (llamado desde NotificationHub) ──────────────────────────

    public void UserConnected(string userId)
        => _state.TryAdd(userId, null);

    public void UserDisconnected(string userId)
        => _state.TryRemove(userId, out _);

    // ── ITaskAssignmentTracker ─────────────────────────────────────────────

    public void AssignTask(string userId, int taskId)
        => _state.AddOrUpdate(userId, taskId, (_, _) => taskId);

    public void ClearTask(string userId)
        => _state.AddOrUpdate(userId, (int?)null, (_, _) => null);

    // ── Worker helpers ─────────────────────────────────────────────────────

    public IReadOnlyCollection<string> GetUsersWithoutTask()
        => _state.Where(kv => !kv.Value.HasValue).Select(kv => kv.Key).ToList();

    public IReadOnlyCollection<int> GetAssignedTaskIds()
        => _state.Values.Where(v => v.HasValue).Select(v => v!.Value).ToList();

    public int ConnectedCount => _state.Count;
}
