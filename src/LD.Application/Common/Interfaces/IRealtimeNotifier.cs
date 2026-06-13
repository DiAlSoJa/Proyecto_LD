using LD.Contracts.SignalR;

namespace LD.Application.Common.Interfaces;

public interface IRealtimeNotifier
{
    Task SendToUserAsync(string userId, HubNotification notification);
    Task SendToGroupAsync(string group, HubNotification notification);
    Task SendToAllAsync(HubNotification notification);
}
