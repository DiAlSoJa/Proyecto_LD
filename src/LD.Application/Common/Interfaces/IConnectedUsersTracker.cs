namespace LD.Application.Common.Interfaces;

public interface IConnectedUsersTracker
{
    IReadOnlyCollection<string> GetConnectedUsers();
    bool IsUserConnected(string userId);
}
