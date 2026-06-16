using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.WarehouseTasks.Commands;

public class CreateWarehouseTaskCommand : WarehouseTaskRequest, IRequest<Result<string>> { }

public class CreateWarehouseTaskCommandHandler : IRequestHandler<CreateWarehouseTaskCommand, Result<string>>
{
    private readonly IRepository<WarehouseTask> _repository;
    private readonly IMapper _mapper;

    public CreateWarehouseTaskCommandHandler(IRepository<WarehouseTask> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<Result<string>> Handle(CreateWarehouseTaskCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Priority))
            return Result<string>.Failure("Selecciona la categoría", new());

        if (string.IsNullOrWhiteSpace(request.Activity))
            return Result<string>.Failure("Selecciona la actividad", new());

        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<string>.Failure("Captura el nombre de la tarea", new());

        if (!request.WarehouseId.HasValue)
            return Result<string>.Failure("Selecciona el almacén", new());

        var task    = _mapper.Map<WarehouseTask>(request);
        var created = await _repository.CreateAsync(task);

        return created
            ? Result<string>.Success("Tarea creada correctamente", string.Empty)
            : Result<string>.Failure("No se pudo crear la tarea", new());
    }
}
