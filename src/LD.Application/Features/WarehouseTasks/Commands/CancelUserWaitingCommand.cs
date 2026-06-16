using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using MediatR;

namespace LD.Application.Features.WarehouseTasks.Commands;

public class CancelUserWaitingCommand : IRequest<Result<string>> { }

public class CancelUserWaitingCommandHandler : IRequestHandler<CancelUserWaitingCommand, Result<string>>
{
    private readonly IConnectedUsersTracker  _tracker;
    private readonly IWarehouseTaskRepository _taskRepo;
    private readonly IUserContextService      _userContext;

    public CancelUserWaitingCommandHandler(
        IConnectedUsersTracker tracker,
        IWarehouseTaskRepository taskRepo,
        IUserContextService userContext)
    {
        _tracker     = tracker;
        _taskRepo    = taskRepo;
        _userContext = userContext;
    }

    public async Task<Result<string>> Handle(CancelUserWaitingCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrEmpty(userId))
            return Result<string>.Failure("Usuario no autenticado", [], 401);

        // Quitar de la lista de disponibles primero para que el worker no le asigne nada nuevo
        _tracker.MarkUserUnavailable(userId);

        // Liberar en BD si el worker ya había reclamado una tarea justo antes de la cancelación
        await _taskRepo.ReleaseTaskForUserAsync(userId, cancellationToken);

        return Result<string>.Success("Espera cancelada", string.Empty);
    }
}
