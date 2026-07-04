using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs.WarehouseTasks;
using LD.Contracts.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LD.Infrastructure.Workers;

/// <summary>
/// Asigna tareas pendientes (NoAsignada) a los usuarios disponibles conectados.
/// Corre en ciclo cada <see cref="CheckInterval"/> tomando un scope nuevo por iteración.
/// </summary>
public sealed class TaskDispatcherWorker : ScopedBackgroundService
{
    private readonly IConnectedUsersTracker         _tracker;
    private readonly IRealtimeNotifier              _notifier;
    private readonly ILogger<TaskDispatcherWorker> _logger;

    private static readonly TimeSpan CheckInterval   = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan StaleTaskWindow = TimeSpan.FromMinutes(5);

    public TaskDispatcherWorker(
        IServiceScopeFactory scopeFactory,
        IConnectedUsersTracker tracker,
        IRealtimeNotifier notifier,
        ILogger<TaskDispatcherWorker> logger)
        : base(scopeFactory)
    {
        _tracker  = tracker;
        _notifier = notifier;
        _logger   = logger;
    }

    protected override TimeSpan GetInterval() => CheckInterval;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        // Libera todas las tareas que quedaron en estado Asignada de sesiones anteriores.
        // Asume instancia única — revisar si se escala horizontalmente.
        using var scope = CreateScope();
        var taskRepo    = scope.ServiceProvider.GetRequiredService<IWarehouseTaskRepository>();
        await taskRepo.ReleaseAllAssignedTasksAsync(cancellationToken);
        _logger.LogInformation("TaskDispatcherWorker startup: tareas huérfanas en estado Asignada liberadas");

        await base.StartAsync(cancellationToken);
    }

    protected override async Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken stoppingToken)
    {
        try
        {
            await DispatchPendingTasksAsync(serviceProvider, stoppingToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // Un ciclo con error no debe tumbar el worker; se reintenta al próximo intervalo.
            _logger.LogError(ex, "Error en ciclo de TaskDispatcherWorker");
        }
    }

    private async Task DispatchPendingTasksAsync(IServiceProvider serviceProvider, CancellationToken ct)
    {
        var availableUsers = _tracker.GetAvailableUsers();
        if (availableUsers.Count == 0)
        {
            _logger.LogDebug("No hay usuarios disponibles — saltando ciclo");
            return;
        }

        var taskRepo = serviceProvider.GetRequiredService<IWarehouseTaskRepository>();
        var mapper   = serviceProvider.GetRequiredService<IMapper>();

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

                await _notifier.SendToUserAsync(userId, new HubNotification
                {
                    Type      = "task_assigned",
                    Title     = $"Nueva tarea: {candidate.Name}",
                    Message   = candidate.Description ?? candidate.Activity,
                    Payload   = JsonSerializer.Serialize(taskDto),
                    CreatedAt = DateTime.UtcNow
                });

                _logger.LogInformation(
                    "Tarea {TaskId} ({TaskName}) asignada a usuario {UserId}",
                    candidate.WarehouseTaskId, candidate.Name, userId);

                break;
            }

            if (candidates.Count == 0) break;
        }
    }
}
