using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.Realtime;
using LD.Contracts.DTOs.WarehouseTasks;
using MediatR;

namespace LD.Application.Features.WarehouseTasks.Queries;

/// <summary>
/// Lista los usuarios conectados al hub SignalR con su nombre, almacenes asignados,
/// el instante de conexión y las tareas que tienen asignadas actualmente.
/// </summary>
public class ConnectedUsersQuery : IRequest<Result<List<ConnectedUserDto>>> { }

public class ConnectedUsersQueryHandler : IRequestHandler<ConnectedUsersQuery, Result<List<ConnectedUserDto>>>
{
    private readonly IConnectedUsersTracker _tracker;
    private readonly IWarehouseTaskRepository _taskRepository;
    private readonly IApplicationUserManager _userManager;
    private readonly IMapper _mapper;

    public ConnectedUsersQueryHandler(
        IConnectedUsersTracker tracker,
        IWarehouseTaskRepository taskRepository,
        IApplicationUserManager userManager,
        IMapper mapper)
    {
        _tracker        = tracker;
        _taskRepository = taskRepository;
        _userManager    = userManager;
        _mapper         = mapper;
    }

    public async Task<Result<List<ConnectedUserDto>>> Handle(ConnectedUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var connectedIds = _tracker.GetConnectedUsers();
            if (connectedIds.Count == 0)
                return Result<List<ConnectedUserDto>>.Success(new(), "No hay usuarios conectados");

            // GetUsersAsync ya trae, por usuario, su info y sus almacenes asignados.
            var users = await _userManager.GetUsersAsync();
            var userLookup = users
                .Where(u => u.User?.Id != null)
                .ToDictionary(u => u.User!.Id!, u => u);

            var assignedTasks = await _taskRepository.GetAssignedTasksForUsersAsync(connectedIds, cancellationToken);
            var tasksByUser = assignedTasks
                .Where(t => t.AssignedToUserId != null)
                .GroupBy(t => t.AssignedToUserId!)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = new List<ConnectedUserDto>();

            foreach (var userId in connectedIds)
            {
                userLookup.TryGetValue(userId, out var userInfo);

                var dto = new ConnectedUserDto
                {
                    UserId      = userId,
                    FullName    = userInfo?.User?.Nombre,
                    UserName    = userInfo?.User?.UserName,
                    ConnectedAt = _tracker.GetConnectedSince(userId),
                    Warehouses  = (userInfo?.Warehouse ?? [])
                        .Select(w => new ConnectedUserWarehouseDto
                        {
                            WarehouseId   = w.Id,
                            WarehouseName = w.NombreAlmacen
                        })
                        .ToList()
                };

                if (tasksByUser.TryGetValue(userId, out var tasks))
                    dto.AssignedTasks = _mapper.Map<List<WarehouseTaskDto>>(tasks);

                result.Add(dto);
            }

            return Result<List<ConnectedUserDto>>.Success(
                result.OrderBy(x => x.FullName ?? x.UserName).ToList(),
                "Usuarios conectados obtenidos correctamente");
        }
        catch (Exception ex)
        {
            return Result<List<ConnectedUserDto>>.Failure(
                "Error al obtener usuarios conectados",
                new List<string> { ex.Message });
        }
    }
}
