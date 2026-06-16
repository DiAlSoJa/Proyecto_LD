using LD.Api.Services;
using LD.Application.Features.OperationalTasks.Commands;
using LD.Application.Features.WarehouseTasks.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LD.Api.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    private readonly ConnectedUsersTracker _tracker;
    private readonly IMediator _mediator;

    public NotificationHub(ConnectedUsersTracker tracker, IMediator mediator)
    {
        _tracker  = tracker;
        _mediator = mediator;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId is not null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            _tracker.UserConnected(userId);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (userId is not null)
        {
            _tracker.UserDisconnected(userId);
            // Libera en BD la tarea asignada al usuario que se desconectó.
            // Cubre desconexión limpia (logout) y reconexión fallida.
            // Desconexión sucia (red caída sin este callback) la maneja ReleaseStaleAssignedTasksAsync en el worker.
            await _mediator.Send(new ReleaseUserTaskCommand { UserId = userId });
            await _mediator.Send(new ReleaseWarehouseTaskForUserCommand { UserId = userId });
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinWarehouseGroup(int warehouseId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, $"warehouse_{warehouseId}");

    public async Task LeaveWarehouseGroup(int warehouseId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"warehouse_{warehouseId}");
}
