using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
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
    private readonly IApplicationUserManager _applicationUserManager;
    private readonly IMapper _mapper;

    public OperationalTaskQueryHandler(
        IOperationalTaskRepository repository,
        IApplicationUserManager applicationUserManager,
        IMapper mapper)
    {
        _repository = repository;
        _applicationUserManager = applicationUserManager;
        _mapper = mapper;
    }

    public async Task<Result<List<OperationalTaskDto>>> Handle(OperationalTaskQuery request, CancellationToken cancellationToken)
    {
        var filtered = await _repository.GetTasksAsync(request.SoloPendientes, request.WarehouseId);
        var dtos = _mapper.Map<List<OperationalTaskDto>>(filtered);
        await FillUserNamesAsync(dtos);

        return Result<List<OperationalTaskDto>>.Success(
            dtos,
            "Tareas obtenidas correctamente");
    }

    private async Task FillUserNamesAsync(List<OperationalTaskDto> tasks)
    {
        var userIds = tasks
            .Select(x => x.CreatedByUserId)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!)
            .Distinct()
            .ToList();

        var users = new Dictionary<string, string>();
        foreach (var userId in userIds)
        {
            var user = await _applicationUserManager.GetUserByIdAsync(userId);
            users[userId] = user?.Username ?? user?.Name ?? userId;
        }

        foreach (var task in tasks)
        {
            task.CreatedByUserName = !string.IsNullOrWhiteSpace(task.CreatedByUserId)
                && users.TryGetValue(task.CreatedByUserId, out var userName)
                    ? userName
                    : task.CreatedByUserId;
        }
    }
}
