using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.OperationalTasks;
using MediatR;

namespace LD.Application.Features.OperationalTasks.Queries;

public class MyAssignedTaskQuery : IRequest<Result<OperationalTaskDto?>> { }

public class MyAssignedTaskQueryHandler : IRequestHandler<MyAssignedTaskQuery, Result<OperationalTaskDto?>>
{
    private readonly IOperationalTaskRepository _taskRepo;
    private readonly IUserContextService _userContext;
    private readonly IMapper _mapper;

    public MyAssignedTaskQueryHandler(
        IOperationalTaskRepository taskRepo,
        IUserContextService userContext,
        IMapper mapper)
    {
        _taskRepo    = taskRepo;
        _userContext = userContext;
        _mapper      = mapper;
    }

    public async Task<Result<OperationalTaskDto?>> Handle(MyAssignedTaskQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrEmpty(userId))
            return Result<OperationalTaskDto?>.Failure("Usuario no autenticado", [], 401);

        //var task = await _taskRepo.GetAssignedTaskForUserAsync(userId, cancellationToken);
        //var dto  = task is not null ? _mapper.Map<OperationalTaskDto>(task) : null;

        return Result<OperationalTaskDto?>.Success(null, "OK");
    }
}
