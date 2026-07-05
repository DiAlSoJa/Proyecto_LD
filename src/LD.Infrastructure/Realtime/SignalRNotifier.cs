using LD.Application.Common.Interfaces;
using LD.Contracts.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace LD.Infrastructure.Realtime;

public sealed class SignalRNotifier : IRealtimeNotifier
{
    private readonly IHubContext<NotificationHub> _hub;

    public SignalRNotifier(IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    public Task SendToUserAsync(string userId, HubNotification notification)
        => _hub.Clients.User(userId).SendAsync("ReceiveNotification", notification);

    public Task SendToGroupAsync(string group, HubNotification notification)
        => _hub.Clients.Group(group).SendAsync("ReceiveNotification", notification);

    public Task SendToAllAsync(HubNotification notification)
        => _hub.Clients.All.SendAsync("ReceiveNotification", notification);
}
