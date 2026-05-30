using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.AvailableInventory;
using MediatR;

namespace LD.Application.Features.AvailableInventories.Queries;

public class AvailableInventoryQuery : IRequest<Result<List<AvailableInventoryDto>?>>
{
    public int? StandardId { get; set; }
}

public class AvailableInventoryQueryHandler : IRequestHandler<AvailableInventoryQuery, Result<List<AvailableInventoryDto>?>>
{
    private readonly IAvailableInventoryRepository _availableInventoryRepository;
    private readonly IApplicationUserManager _applicationUserManager;
    private readonly IMapper _mapper;

    public AvailableInventoryQueryHandler(
        IAvailableInventoryRepository availableInventoryRepository,
        IApplicationUserManager applicationUserManager,
        IMapper mapper)
    {
        _availableInventoryRepository = availableInventoryRepository;
        _applicationUserManager = applicationUserManager;
        _mapper = mapper;
    }

    public async Task<Result<List<AvailableInventoryDto>?>> Handle(AvailableInventoryQuery request, CancellationToken cancellationToken)
    {
        var availableInventories = await _availableInventoryRepository.GetAllWithRelationsAsync(request.StandardId);
        var availableInventoryDtos = _mapper.Map<List<AvailableInventoryDto>>(availableInventories);
        await FillUserNamesAsync(availableInventoryDtos);

        return Result<List<AvailableInventoryDto>?>.Success(availableInventoryDtos, "Inventario disponible obtenido correctamente");
    }

    private async Task FillUserNamesAsync(List<AvailableInventoryDto> inventories)
    {
        var userIds = inventories
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

        foreach (var inventory in inventories)
        {
            inventory.UserName = users.TryGetValue(inventory.UserId, out var userName)
                ? userName
                : inventory.UserId;
        }
    }
}
