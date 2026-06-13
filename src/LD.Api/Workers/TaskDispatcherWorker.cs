using AutoMapper;
using LD.Api.Hubs;
using LD.Api.Services;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs.OperationalTasks;
using LD.Contracts.SignalR;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace LD.Api.Workers;

/// <summary>
/// Worker de pruebas que cada 30 segundos revisa qué usuarios están conectados sin tarea
/// asignada y les despacha una tarea pendiente de la base de datos.
///
/// Flujo completo:
///   1. Usuario conecta → Hub → ConnectedUsersTracker.UserConnected
///   2. Worker detecta usuario sin tarea → asigna una desde DB → envía por SignalR
///   3. Usuario completa tarea → CompleteOperationalTaskCommand → publica TaskCompletedNotification
///   4. TaskCompletedNotificationHandler → busca siguiente tarea → envía por IRealtimeNotifier
/// </summary>
public sealed class TaskDispatcherWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ConnectedUsersTracker _tracker;
    private readonly IHubContext<NotificationHub> _hub;
    private readonly ILogger<TaskDispatcherWorker> _logger;

    private static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(30);

    public TaskDispatcherWorker(
        IServiceScopeFactory scopeFactory,
        ConnectedUsersTracker tracker,
        IHubContext<NotificationHub> hub,
        ILogger<TaskDispatcherWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _tracker      = tracker;
        _hub          = hub;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TaskDispatcherWorker iniciado — intervalo {Interval}s", CheckInterval.TotalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DispatchPendingTasksAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error en ciclo de TaskDispatcherWorker");
            }

            await Task.Delay(CheckInterval, stoppingToken);
        }
    }

    private async Task DispatchPendingTasksAsync(CancellationToken ct)
    {
        var usersNeedingTask = _tracker.GetUsersWithoutTask();
        if (usersNeedingTask.Count == 0)
            return;

        _logger.LogDebug("{Count} usuario(s) conectados sin tarea — buscando tareas pendientes", usersNeedingTask.Count);

        using var scope   = _scopeFactory.CreateScope();
        var taskRepo      = scope.ServiceProvider.GetRequiredService<IOperationalTaskRepository>();
        var mapper        = scope.ServiceProvider.GetRequiredService<IMapper>();

        var pending       = await taskRepo.GetTasksAsync(soloPendientes: true, warehouseId: null);
        var alreadyTaken  = _tracker.GetAssignedTaskIds().ToHashSet();

        // Candidatas: pendientes que no están ya asignadas a nadie
        var candidates = pending
            .Where(t => !alreadyTaken.Contains(t.OperationalTaskId))
            .OrderBy(_ => Guid.NewGuid())   // Orden aleatorio para variedad
            .ToList();

        if (candidates.Count == 0)
        {
            _logger.LogDebug("No hay tareas candidatas disponibles para despachar");
            return;
        }

        foreach (var userId in usersNeedingTask)
        {
            if (candidates.Count == 0) break;

            var task = candidates[0];
            candidates.RemoveAt(0);

            _tracker.AssignTask(userId, task.OperationalTaskId);

            var taskDto = mapper.Map<OperationalTaskDto>(task);

            await _hub.Clients.User(userId).SendAsync("ReceiveNotification", new HubNotification
            {
                Type      = "task_assigned",
                Title     = $"Nueva tarea: {task.Name}",
                Message   = task.Description ?? task.Activity,
                Payload   = JsonSerializer.Serialize(taskDto),
                CreatedAt = DateTime.UtcNow
            }, ct);

            _logger.LogInformation(
                "Tarea {TaskId} ({TaskName}) despachada a usuario {UserId}",
                task.OperationalTaskId, task.Name, userId);
        }
    }
}
