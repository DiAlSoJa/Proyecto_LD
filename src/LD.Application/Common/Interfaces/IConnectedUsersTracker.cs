namespace LD.Application.Common.Interfaces;

public interface IConnectedUsersTracker
{
    IReadOnlyCollection<string> GetConnectedUsers();
    bool IsUserConnected(string userId);

    // Momento (UTC) en que el usuario estableció su conexión actual, o null si no está conectado.
    DateTime? GetConnectedSince(string userId);

    // Disponibilidad para recibir tareas (estado transitorio en memoria)
    void MarkUserAvailable(string userId);
    void MarkUserUnavailable(string userId);
    IReadOnlyCollection<string> GetAvailableUsers();
    bool IsUserAvailable(string userId);
}
