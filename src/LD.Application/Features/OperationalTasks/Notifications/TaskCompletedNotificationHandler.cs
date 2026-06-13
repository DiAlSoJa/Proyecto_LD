using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs.OperationalTasks;
using LD.Contracts.SignalR;
using MediatR;
using System.Text.Json;

namespace LD.Application.Features.OperationalTasks.Notifications;

public class TaskCompletedNotificationHandler : INotificationHandler<TaskCompletedNotification>
{
    private readonly IOperationalTaskRepository _taskRepo;
    private readonly IRealtimeNotifier _notifier;
    private readonly ITaskAssignmentTracker _tracker;
    private readonly IMapper _mapper;

    public TaskCompletedNotificationHandler(
        IOperationalTaskRepository taskRepo,
        IRealtimeNotifier notifier,
        ITaskAssignmentTracker tracker,
        IMapper mapper)
    {
        _taskRepo = taskRepo;
        _notifier = notifier;
        _tracker  = tracker;
        _mapper   = mapper;
    }

    public async Task Handle(TaskCompletedNotification notification, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(notification.CompletedByUserId))
            return;

        // Liberar el slot del usuario para que el worker no lo cuente como ocupado
        _tracker.ClearTask(notification.CompletedByUserId);

        var pendingTasks = await _taskRepo.GetTasksAsync(soloPendientes: true, warehouseId: null);

        // Tomar una tarea pendiente diferente a la que acaba de terminar
        var nextTask = pendingTasks
            .Where(t => t.OperationalTaskId != notification.CompletedTaskId)
            .OrderBy(_ => Guid.NewGuid())
            .FirstOrDefault();

        if (nextTask is null)
            return;

        _tracker.AssignTask(notification.CompletedByUserId, nextTask.OperationalTaskId);

        var taskDto = _mapper.Map<OperationalTaskDto>(nextTask);

        await _notifier.SendToUserAsync(notification.CompletedByUserId, new HubNotification
        {
            Type      = "task_assigned",
            Title     = $"Nueva tarea: {nextTask.Name}",
            Message   = nextTask.Description ?? nextTask.Activity,
            Payload   = JsonSerializer.Serialize(taskDto),
            CreatedAt = DateTime.UtcNow
        });
    }
}
