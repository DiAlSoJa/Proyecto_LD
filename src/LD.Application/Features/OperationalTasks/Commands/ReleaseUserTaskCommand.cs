using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using MediatR;

namespace LD.Application.Features.OperationalTasks.Commands;

public class ReleaseUserTaskCommand : IRequest<Result<string>>
{
    public string UserId { get; init; } = string.Empty;
}

public class ReleaseUserTaskCommandHandler : IRequestHandler<ReleaseUserTaskCommand, Result<string>>
{
    private readonly IOperationalTaskRepository _taskRepo;

    public ReleaseUserTaskCommandHandler(IOperationalTaskRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public async Task<Result<string>> Handle(ReleaseUserTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _taskRepo.ReleaseTaskForUserAsync(request.UserId, cancellationToken);
            return Result<string>.Success("Tarea liberada", string.Empty);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Error al liberar la tarea", [ex.Message]);
        }
    }
}
