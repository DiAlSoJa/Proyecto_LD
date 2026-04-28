using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.InventoryMovement;
using MediatR;

namespace LD.Application.Features.InventoryMovement.Queries;

public class InventoryMovementQuery : IRequest<Result<List<InventoryMovementDto>?>>
{
}

public class InventoryMovementQueryHandler : IRequestHandler<InventoryMovementQuery, Result<List<InventoryMovementDto>?>>
{
    private readonly IInventoryMovementRepository _inventoryMovementRepository;
    private readonly IMapper _mapper;

    public InventoryMovementQueryHandler(IInventoryMovementRepository inventoryMovementRepository, IMapper mapper)
    {
        _inventoryMovementRepository = inventoryMovementRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<InventoryMovementDto>?>> Handle(InventoryMovementQuery request, CancellationToken cancellationToken)
    {
        var inventoryMovements = await _inventoryMovementRepository.GetAllWithRelationsAsync();
        var inventoryMovementDtos = _mapper.Map<List<InventoryMovementDto>>(
            inventoryMovements.OrderByDescending(x => x.MovementId));
        return Result<List<InventoryMovementDto>?>.Success(inventoryMovementDtos, "Movimientos obtenidos correctamente");
    }
}
