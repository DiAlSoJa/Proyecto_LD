using AutoMapper;
using LD.Api.Hubs;
using LD.Api.Services;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs.WarehouseTasks;
using LD.Contracts.SignalR;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace LD.Api.Workers;

public sealed class TaskDispatcherWorker : BackgroundService
{
    private readonly IServiceScopeFactory          _scopeFactory;
    private readonly ConnectedUsersTracker          _tracker;
    private readonly IHubContext<NotificationHub>  _hub;
    private readonly ILogger<TaskDispatcherWorker> _logger;

    private static readonly TimeSpan CheckInterval   = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan StaleTaskWindow = TimeSpan.FromMinutes(5);

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

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        // Libera todas las tareas que quedaron en estado Asignada de sesiones anteriores.
        // Asume instancia única — revisar si se escala horizontalmente.
        using var scope  = _scopeFactory.CreateScope();
        var taskRepo     = scope.ServiceProvider.GetRequiredService<IWarehouseTaskRepository>();
        await taskRepo.ReleaseAllAssignedTasksAsync(cancellationToken);
        _logger.LogInformation("TaskDispatcherWorker startup: tareas huérfanas en estado Asignada liberadas");

        await base.StartAsync(cancellationToken);
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
        var availableUsers = _tracker.GetAvailableUsers();
        if (availableUsers.Count == 0)
        {
            _logger.LogDebug("No hay usuarios disponibles — saltando ciclo");
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var taskRepo    = scope.ServiceProvider.GetRequiredService<IWarehouseTaskRepository>();
        var mapper      = scope.ServiceProvider.GetRequiredService<IMapper>();

        // Limpia tareas cuyo usuario ya no está conectado o llevan más de StaleTaskWindow asignadas.
        var connectedUsers = _tracker.GetConnectedUsers();
        await taskRepo.ReleaseStaleAssignedTasksAsync(connectedUsers, StaleTaskWindow, ct);

        // Filtra usuarios disponibles que aún no tienen tarea asignada en BD.
        var usersWithTask    = await taskRepo.GetUserIdsWithAssignedTaskAsync(availableUsers, ct);
        var usersNeedingTask = availableUsers.Where(u => !usersWithTask.Contains(u)).ToList();

        if (usersNeedingTask.Count == 0)
        {
            _logger.LogDebug("Todos los usuarios disponibles ya tienen tarea asignada");
            return;
        }

        _logger.LogDebug("{Count} usuario(s) disponible(s) sin tarea — buscando candidatas", usersNeedingTask.Count);

        // Candidatas en orden de llegada (FIFO). Barajamos dentro del FIFO para
        // evitar que todos los workers intenten siempre la misma tarea primero.
        var candidates = (await taskRepo.GetPendingUnassignedTasksAsync(ct))
            .OrderBy(_ => Guid.NewGuid())
            .ToList();

        if (candidates.Count == 0)
        {
            _logger.LogDebug("No hay tareas candidatas (NoAsignada) disponibles");
            return;
        }

        foreach (var userId in usersNeedingTask)
        {
            foreach (var candidate in candidates)
            {
                // TryClaimTaskAsync es atómico: UPDATE WHERE Status=NoAsignada.
                // Si otro proceso ya la reclamó, devuelve false y probamos la siguiente.
                var claimed = await taskRepo.TryClaimTaskAsync(candidate.WarehouseTaskId, userId, ct);
                if (!claimed)
                    continue;

                // Remover del set de disponibles para que el siguiente ciclo no re-intente
                _tracker.MarkUserUnavailable(userId);

                // Eliminar la candidata reclamada de la lista local para no asignarla a otro
                candidates.Remove(candidate);

                var taskDto = mapper.Map<WarehouseTaskDto>(candidate);

                await _hub.Clients.User(userId).SendAsync("ReceiveNotification", new HubNotification
                {
                    Type      = "task_assigned",
                    Title     = $"Nueva tarea: {candidate.Name}",
                    Message   = candidate.Description ?? candidate.Activity,
                    Payload   = JsonSerializer.Serialize(taskDto),
                    CreatedAt = DateTime.UtcNow
                }, ct);

                _logger.LogInformation(
                    "Tarea {TaskId} ({TaskName}) asignada a usuario {UserId}",
                    candidate.WarehouseTaskId, candidate.Name, userId);

                break;
            }

            if (candidates.Count == 0) break;
        }
    }
}
