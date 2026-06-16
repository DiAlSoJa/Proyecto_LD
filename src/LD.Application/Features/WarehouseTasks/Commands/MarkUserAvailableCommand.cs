using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using MediatR;

namespace LD.Application.Features.WarehouseTasks.Commands;

public class MarkUserAvailableCommand : IRequest<Result<string>> { }

public class MarkUserAvailableCommandHandler : IRequestHandler<MarkUserAvailableCommand, Result<string>>
{
    private readonly IConnectedUsersTracker _tracker;
    private readonly IUserContextService    _userContext;

    public MarkUserAvailableCommandHandler(IConnectedUsersTracker tracker, IUserContextService userContext)
    {
        _tracker     = tracker;
        _userContext = userContext;
    }

    public Task<Result<string>> Handle(MarkUserAvailableCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrEmpty(userId))
            return Task.FromResult(Result<string>.Failure("Usuario no autenticado", [], 401));

        _tracker.MarkUserAvailable(userId);
        return Task.FromResult(Result<string>.Success("Disponible para recibir tareas", string.Empty));
    }
}
