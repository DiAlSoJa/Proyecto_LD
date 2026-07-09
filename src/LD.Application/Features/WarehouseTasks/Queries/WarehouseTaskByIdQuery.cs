using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.WarehouseTasks;
using MediatR;

namespace LD.Application.Features.WarehouseTasks.Queries;

public class WarehouseTaskByIdQuery : IRequest<Result<WarehouseTaskDto>>
{
    public int WarehouseTaskId { get; set; }
}

public class WarehouseTaskByIdQueryHandler : IRequestHandler<WarehouseTaskByIdQuery, Result<WarehouseTaskDto>>
{
    private readonly IWarehouseTaskRepository _repository;
    private readonly IMapper _mapper;

    public WarehouseTaskByIdQueryHandler(IWarehouseTaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<Result<WarehouseTaskDto>> Handle(WarehouseTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetTaskByIdAsync(request.WarehouseTaskId);
        if (task is null)
            return Result<WarehouseTaskDto>.Failure("No se encontró la tarea", new());

        return Result<WarehouseTaskDto>.Success(_mapper.Map<WarehouseTaskDto>(task), "Tarea obtenida correctamente");
    }
}
