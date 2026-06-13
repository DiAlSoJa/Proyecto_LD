using LD.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LD.Api.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    private readonly ConnectedUsersTracker _tracker;

    public NotificationHub(ConnectedUsersTracker tracker)
    {
        _tracker = tracker;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId is not null)
        {
            // Grupo personal para envíos directos por userId
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            _tracker.UserConnected(userId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (userId is not null)
            _tracker.UserDisconnected(userId);

        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinWarehouseGroup(int warehouseId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, $"warehouse_{warehouseId}");

    public async Task LeaveWarehouseGroup(int warehouseId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"warehouse_{warehouseId}");
}
