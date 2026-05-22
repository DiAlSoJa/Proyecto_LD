using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.OperationalTasks.Commands;

public class CreateOperationalTaskCommand : OperationalTaskRequest, IRequest<Result<string>>
{
}

public class CreateOperationalTaskCommandHandler : IRequestHandler<CreateOperationalTaskCommand, Result<string>>
{
    private readonly IRepository<OperationalTask> _repository;
    private readonly IMapper _mapper;

    public CreateOperationalTaskCommandHandler(IRepository<OperationalTask> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateOperationalTaskCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Priority))
            return Result<string>.Failure("Selecciona la categoria", new());

        if (string.IsNullOrWhiteSpace(request.Activity))
            return Result<string>.Failure("Selecciona la actividad", new());

        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<string>.Failure("Captura el nombre de la tarea", new());

        if (!request.WarehouseId.HasValue)
            return Result<string>.Failure("Selecciona el almacen", new());

        var task = _mapper.Map<OperationalTask>(request);
        var created = await _repository.CreateAsync(task);

        return created
            ? Result<string>.Success("Tarea creada correctamente", string.Empty)
            : Result<string>.Failure("No se pudo crear la tarea", new());
    }
}
