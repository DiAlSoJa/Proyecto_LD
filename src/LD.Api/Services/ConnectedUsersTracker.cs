using LD.Application.Common.Interfaces;
using System.Collections.Concurrent;

namespace LD.Api.Services;

/// <summary>
/// Singleton que rastrea qué usuarios están conectados al hub SignalR.
/// La fuente de verdad de qué tarea tiene cada usuario vive en la BD (OperationalTask.AssignedToUserId).
/// Este tracker solo es caché ligero para saber a quién enviar notificaciones.
/// </summary>
public sealed class ConnectedUsersTracker : IConnectedUsersTracker
{
    // userId → byte (la presencia es lo único que importa; byte consume mínima memoria)
    private readonly ConcurrentDictionary<string, byte> _connected = new();

    public void UserConnected(string userId)
        => _connected.TryAdd(userId, 0);

    public void UserDisconnected(string userId)
        => _connected.TryRemove(userId, out _);

    public IReadOnlyCollection<string> GetConnectedUsers()
        => _connected.Keys.ToList();

    public bool IsUserConnected(string userId)
        => _connected.ContainsKey(userId);

    public int ConnectedCount => _connected.Count;
}
