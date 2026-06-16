using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Application.Features.OperationalTasks.Notifications;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.OperationalTasks.Commands;

public class CompleteOperationalTaskCommand : CompleteOperationalTaskRequest, IRequest<Result<string>>
{
    public int OperationalTaskId { get; set; }
}

public class CompleteOperationalTaskCommandHandler : IRequestHandler<CompleteOperationalTaskCommand, Result<string>>
{
    private readonly IRepository<OperationalTask> _repository;
    private readonly IPublisher _publisher;
    private readonly IUserContextService _userContext;

    public CompleteOperationalTaskCommandHandler(
        IRepository<OperationalTask> repository,
        IPublisher publisher,
        IUserContextService userContext)
    {
        _repository  = repository;
        _publisher   = publisher;
        _userContext = userContext;
    }

    public async Task<Result<string>> Handle(CompleteOperationalTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(request.OperationalTaskId);
        if (task is null)
            return Result<string>.Failure("No se encontró la tarea", []);

        var currentUserId = _userContext.UserId ?? string.Empty;

        // Valida que la tarea siga asignada a quien la intenta completar.
        // Evita pisar el estado si ya fue liberada por stale-timeout y reasignada a otro usuario.
        //if (!string.IsNullOrEmpty(task.AssignedToUserId) && task.AssignedToUserId != currentUserId)
        //    return Result<string>.Failure(
        //        "Esta tarea ya no está asignada a tu usuario. Es posible que haya sido reasignada por inactividad.",
        //        [], 409);

        //task.Status                 = OperationalTaskStatus.Completada;
        //task.AssignedToUserId       = null;
        //task.AssignedAt             = null;
        //task.Completed              = true;
        //task.CompletedAt            = DateTime.UtcNow;
        //task.CompletedByName        = request.CompletedBy;
        //task.CompletedByUserId      = currentUserId;
        //task.ResolutionObservations = request.ResolutionObservations;
        //task.ResolvedPhoto1Path     = request.ResolvedPhoto1Path;
        //task.ResolvedPhoto2Path     = request.ResolvedPhoto2Path;
        //task.ResolvedPhoto3Path     = request.ResolvedPhoto3Path;
        //task.ResolvedPhoto4Path     = request.ResolvedPhoto4Path;

        var updated = await _repository.UpdateAsync(task);
        if (!updated)
            return Result<string>.Failure("No se pudo terminar la tarea", []);

        await _publisher.Publish(new TaskCompletedNotification
        {
            CompletedTaskId   = request.OperationalTaskId,
            CompletedByUserId = currentUserId
        }, cancellationToken);

        return Result<string>.Success("Tarea terminada correctamente", string.Empty);
    }
}
