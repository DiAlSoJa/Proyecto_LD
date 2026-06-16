using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.WarehouseTasks;
using MediatR;

namespace LD.Application.Features.WarehouseTasks.Queries;

public class MyAssignedWarehouseTaskQuery : IRequest<Result<WarehouseTaskDto?>> { }

public class MyAssignedWarehouseTaskQueryHandler : IRequestHandler<MyAssignedWarehouseTaskQuery, Result<WarehouseTaskDto?>>
{
    private readonly IWarehouseTaskRepository _taskRepo;
    private readonly IUserContextService      _userContext;
    private readonly IMapper                  _mapper;

    public MyAssignedWarehouseTaskQueryHandler(
        IWarehouseTaskRepository taskRepo,
        IUserContextService userContext,
        IMapper mapper)
    {
        _taskRepo    = taskRepo;
        _userContext = userContext;
        _mapper      = mapper;
    }

    public async Task<Result<WarehouseTaskDto?>> Handle(MyAssignedWarehouseTaskQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrEmpty(userId))
            return Result<WarehouseTaskDto?>.Failure("Usuario no autenticado", [], 401);

        var task = await _taskRepo.GetAssignedTaskForUserAsync(userId, cancellationToken);
        var dto  = task is not null ? _mapper.Map<WarehouseTaskDto>(task) : null;

        return Result<WarehouseTaskDto?>.Success(dto, "OK");
    }
}
