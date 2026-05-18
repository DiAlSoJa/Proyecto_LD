using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.OperationalTasks;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.OperationalTasks.Queries;

public class OperationalTaskByIdQuery : IRequest<Result<OperationalTaskDto>>
{
    public int OperationalTaskId { get; set; }
}

public class OperationalTaskByIdQueryHandler : IRequestHandler<OperationalTaskByIdQuery, Result<OperationalTaskDto>>
{
    private readonly IOperationalTaskRepository _repository;
    private readonly IMapper _mapper;

    public OperationalTaskByIdQueryHandler(IOperationalTaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<OperationalTaskDto>> Handle(OperationalTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetTaskByIdAsync(request.OperationalTaskId);
        if (task is null)
            return Result<OperationalTaskDto>.Failure("No se encontro la tarea", new());

        return Result<OperationalTaskDto>.Success(_mapper.Map<OperationalTaskDto>(task), "Tarea obtenida correctamente");
    }
}
