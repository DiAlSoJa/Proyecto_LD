using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.OperationalTasks.Commands;

public class CompleteOperationalTaskCommand : CompleteOperationalTaskRequest, IRequest<Result<string>>
{
    public int OperationalTaskId { get; set; }
}

public class CompleteOperationalTaskCommandHandler : IRequestHandler<CompleteOperationalTaskCommand, Result<string>>
{
    private readonly IRepository<OperationalTask> _repository;

    public CompleteOperationalTaskCommandHandler(IRepository<OperationalTask> repository)
    {
        _repository = repository;
    }

    public async Task<Result<string>> Handle(CompleteOperationalTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(request.OperationalTaskId);
        if (task is null)
            return Result<string>.Failure("No se encontro la tarea", new());

        task.Completed = true;
        task.CompletedAt = DateTime.UtcNow;
        task.CompletedBy = request.CompletedBy;
        task.ResolutionObservations = request.ResolutionObservations;
        task.ResolvedPhoto1Path = request.ResolvedPhoto1Path;
        task.ResolvedPhoto2Path = request.ResolvedPhoto2Path;
        task.ResolvedPhoto3Path = request.ResolvedPhoto3Path;
        task.ResolvedPhoto4Path = request.ResolvedPhoto4Path;

        var updated = await _repository.UpdateAsync(task);
        return updated
            ? Result<string>.Success("Tarea terminada correctamente", string.Empty)
            : Result<string>.Failure("No se pudo terminar la tarea", new());
    }
}
