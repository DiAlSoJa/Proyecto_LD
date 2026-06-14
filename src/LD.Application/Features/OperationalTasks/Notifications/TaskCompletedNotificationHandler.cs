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
    private readonly IMapper _mapper;

    public TaskCompletedNotificationHandler(
        IOperationalTaskRepository taskRepo,
        IRealtimeNotifier notifier,
        IMapper mapper)
    {
        _taskRepo = taskRepo;
        _notifier = notifier;
        _mapper   = mapper;
    }

    public async Task Handle(TaskCompletedNotification notification, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(notification.CompletedByUserId))
            return;

        var userId = notification.CompletedByUserId;

        // Obtener candidatas en orden aleatorio para variedad
        var candidates = await _taskRepo.GetPendingUnassignedTasksAsync(cancellationToken);
        var shuffled   = candidates
            .Where(t => t.OperationalTaskId != notification.CompletedTaskId)
            .OrderBy(_ => Guid.NewGuid())
            .ToList();

        foreach (var candidate in shuffled)
        {
            // TryClaimTaskAsync es atómico: UPDATE WHERE Status=NoAsignada
            // Si otro hilo/worker ya la tomó, devuelve false y probamos la siguiente
            var claimed = await _taskRepo.TryClaimTaskAsync(candidate.OperationalTaskId, userId, cancellationToken);
            if (!claimed)
                continue;

            var taskDto = _mapper.Map<OperationalTaskDto>(candidate);

            await _notifier.SendToUserAsync(userId, new HubNotification
            {
                Type      = "task_assigned",
                Title     = $"Nueva tarea: {candidate.Name}",
                Message   = candidate.Description ?? candidate.Activity,
                Payload   = JsonSerializer.Serialize(taskDto),
                CreatedAt = DateTime.UtcNow
            });

            return;
        }
    }
}
