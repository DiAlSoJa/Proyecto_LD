using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using MediatR;

namespace LD.Application.Features.WarehouseTasks.Commands;

public class ReleaseWarehouseTaskForUserCommand : IRequest<Result<string>>
{
    public string UserId { get; init; } = string.Empty;
}

public class ReleaseWarehouseTaskForUserCommandHandler : IRequestHandler<ReleaseWarehouseTaskForUserCommand, Result<string>>
{
    private readonly IWarehouseTaskRepository _taskRepo;

    public ReleaseWarehouseTaskForUserCommandHandler(IWarehouseTaskRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public async Task<Result<string>> Handle(ReleaseWarehouseTaskForUserCommand request, CancellationToken cancellationToken)
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
