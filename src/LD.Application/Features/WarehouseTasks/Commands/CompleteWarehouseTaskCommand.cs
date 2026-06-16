using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.WarehouseTasks.Commands;

public class CompleteWarehouseTaskCommand : CompleteWarehouseTaskRequest, IRequest<Result<string>>
{
    public int WarehouseTaskId { get; set; }
}

public class CompleteWarehouseTaskCommandHandler : IRequestHandler<CompleteWarehouseTaskCommand, Result<string>>
{
    private readonly IWarehouseTaskRepository _taskRepo;
    private readonly IConnectedUsersTracker   _tracker;
    private readonly IUserContextService      _userContext;

    public CompleteWarehouseTaskCommandHandler(
        IWarehouseTaskRepository taskRepo,
        IConnectedUsersTracker tracker,
        IUserContextService userContext)
    {
        _taskRepo    = taskRepo;
        _tracker     = tracker;
        _userContext = userContext;
    }

    public async Task<Result<string>> Handle(CompleteWarehouseTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepo.GetByIdAsync(request.WarehouseTaskId);
        if (task is null)
            return Result<string>.Failure("No se encontró la tarea", []);

        var currentUserId = _userContext.UserId ?? string.Empty;

        // Evita que otro usuario complete una tarea que no le fue asignada.
        if (!string.IsNullOrEmpty(task.AssignedToUserId) && task.AssignedToUserId != currentUserId)
            return Result<string>.Failure(
                "Esta tarea está asignada a otro usuario.",
                [], 409);

        task.Status                 = WarehouseTaskStatus.Completada;
        task.AssignedToUserId       = null;
        task.AssignedAt             = null;
        task.CompletedAt            = DateTime.UtcNow;
        task.CompletedByUserId      = currentUserId;
        task.CompletedByName        = request.CompletedByName;
        task.ResolutionObservations = request.ResolutionObservations;
        task.ResolvedPhoto1Path     = request.ResolvedPhoto1Path;
        task.ResolvedPhoto2Path     = request.ResolvedPhoto2Path;
        task.ResolvedPhoto3Path     = request.ResolvedPhoto3Path;
        task.ResolvedPhoto4Path     = request.ResolvedPhoto4Path;

        var updated = await _taskRepo.UpdateAsync(task);
        if (!updated)
            return Result<string>.Failure("No se pudo terminar la tarea", []);

        // El usuario completó — ya no está disponible para recibir otra tarea automáticamente.
        // Tendrá que volver a la pantalla de espera si quiere más.
        _tracker.MarkUserUnavailable(currentUserId);

        return Result<string>.Success("Tarea terminada correctamente", string.Empty);
    }
}
