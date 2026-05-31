using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.InventoryMovement;
using MediatR;

namespace LD.Application.Features.InventoryMovement.Queries;

public class InventoryMovementQuery : IRequest<Result<List<InventoryMovementDto>?>>
{
    public int? StandardId { get; set; }
    public string? StandardIdCode { get; set; }
}

public class InventoryMovementQueryHandler : IRequestHandler<InventoryMovementQuery, Result<List<InventoryMovementDto>?>>
{
    private readonly IInventoryMovementRepository _inventoryMovementRepository;
    private readonly IApplicationUserManager _applicationUserManager;
    private readonly IMapper _mapper;

    public InventoryMovementQueryHandler(
        IInventoryMovementRepository inventoryMovementRepository,
        IApplicationUserManager applicationUserManager,
        IMapper mapper)
    {
        _inventoryMovementRepository = inventoryMovementRepository;
        _applicationUserManager = applicationUserManager;
        _mapper = mapper;
    }

    public async Task<Result<List<InventoryMovementDto>?>> Handle(InventoryMovementQuery request, CancellationToken cancellationToken)
    {
        var inventoryMovements = await _inventoryMovementRepository.GetAllWithRelationsAsync(
            request.StandardId,
            request.StandardIdCode);
        var inventoryMovementDtos = _mapper.Map<List<InventoryMovementDto>>(
            inventoryMovements.OrderByDescending(x => x.MovementId));

        await FillUserNamesAsync(inventoryMovementDtos);

        return Result<List<InventoryMovementDto>?>.Success(inventoryMovementDtos, "Movimientos obtenidos correctamente");
    }

    private async Task FillUserNamesAsync(List<InventoryMovementDto> movements)
    {
        var userIds = movements
            .Select(x => x.UserId)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList();

        var users = new Dictionary<string, string>();
        foreach (var userId in userIds)
        {
            var user = await _applicationUserManager.GetUserByIdAsync(userId);
            users[userId] = user?.Username ?? user?.Name ?? userId;
        }

        foreach (var movement in movements)
        {
            movement.UserName = users.TryGetValue(movement.UserId, out var userName)
                ? userName
                : movement.UserId;
        }
    }
}
