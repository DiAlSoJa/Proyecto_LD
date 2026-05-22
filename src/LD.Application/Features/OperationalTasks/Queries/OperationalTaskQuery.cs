using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.OperationalTasks;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.OperationalTasks.Queries;

public class OperationalTaskQuery : IRequest<Result<List<OperationalTaskDto>>>
{
    public bool SoloPendientes { get; set; }
    public int? WarehouseId { get; set; }
}

public class OperationalTaskQueryHandler : IRequestHandler<OperationalTaskQuery, Result<List<OperationalTaskDto>>>
{
    private readonly IOperationalTaskRepository _repository;
    private readonly IMapper _mapper;

    public OperationalTaskQueryHandler(IOperationalTaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<OperationalTaskDto>>> Handle(OperationalTaskQuery request, CancellationToken cancellationToken)
    {
        var filtered = await _repository.GetTasksAsync(request.SoloPendientes, request.WarehouseId);

        return Result<List<OperationalTaskDto>>.Success(
            _mapper.Map<List<OperationalTaskDto>>(filtered),
            "Tareas obtenidas correctamente");
    }
}
