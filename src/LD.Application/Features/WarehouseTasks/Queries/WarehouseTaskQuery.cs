using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.WarehouseTasks;
using MediatR;

namespace LD.Application.Features.WarehouseTasks.Queries;

public class WarehouseTaskQuery : IRequest<Result<List<WarehouseTaskDto>>>
{
    public bool SoloPendientes { get; set; }
    public int? WarehouseId { get; set; }
}

public class WarehouseTaskQueryHandler : IRequestHandler<WarehouseTaskQuery, Result<List<WarehouseTaskDto>>>
{
    private readonly IWarehouseTaskRepository _repository;
    private readonly IMapper _mapper;

    public WarehouseTaskQueryHandler(IWarehouseTaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<Result<List<WarehouseTaskDto>>> Handle(WarehouseTaskQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetTasksAsync(request.SoloPendientes, request.WarehouseId);
        return Result<List<WarehouseTaskDto>>.Success(
            _mapper.Map<List<WarehouseTaskDto>>(tasks),
            "Tareas obtenidas correctamente");
    }
}
