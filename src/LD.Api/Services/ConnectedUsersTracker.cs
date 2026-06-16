using LD.Application.Common.Interfaces;
using System.Collections.Concurrent;

namespace LD.Api.Services;

/// <summary>
/// Singleton que rastrea qué usuarios están conectados al hub SignalR
/// y cuáles están disponibles para recibir tareas de almacén.
///
/// La fuente de verdad de qué tarea tiene cada usuario vive en la BD (WarehouseTask.AssignedToUserId).
/// Este tracker es caché ligero para routing de notificaciones y para que el
/// TaskDispatcherWorker sepa a quién asignar sin consultar la BD en cada ciclo.
/// </summary>
public sealed class ConnectedUsersTracker : IConnectedUsersTracker
{
    // userId → byte (la presencia es lo único que importa; byte consume mínima memoria)
    private readonly ConcurrentDictionary<string, byte> _connected        = new();
    private readonly ConcurrentDictionary<string, byte> _availableForTask = new();

    // ── Conexión ──────────────────────────────────────────────────────────────

    public void UserConnected(string userId)
        => _connected.TryAdd(userId, 0);

    public void UserDisconnected(string userId)
    {
        _connected.TryRemove(userId, out _);
        // Al desconectarse, el usuario deja de estar disponible automáticamente.
        _availableForTask.TryRemove(userId, out _);
    }

    public IReadOnlyCollection<string> GetConnectedUsers()
        => _connected.Keys.ToList();

    public bool IsUserConnected(string userId)
        => _connected.ContainsKey(userId);

    public int ConnectedCount => _connected.Count;

    // ── Disponibilidad para recibir tareas ────────────────────────────────────

    public void MarkUserAvailable(string userId)
        => _availableForTask.TryAdd(userId, 0);

    public void MarkUserUnavailable(string userId)
        => _availableForTask.TryRemove(userId, out _);

    public IReadOnlyCollection<string> GetAvailableUsers()
        => _availableForTask.Keys.ToList();

    public bool IsUserAvailable(string userId)
        => _availableForTask.ContainsKey(userId);
}
