using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.InventoryMovement.Queries;

public record InventoryMovementByIdQuery(int MovementId)
    : IRequest<Result<InventoryMovementRequest?>>;

public class InventoryMovementByIdQueryHandler : IRequestHandler<InventoryMovementByIdQuery, Result<InventoryMovementRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.InventoryMovement> _inventoryMovementRepository;
    private readonly IMapper _mapper;

    public InventoryMovementByIdQueryHandler(IRepository<LD.Domain.Entities.InventoryMovement> inventoryMovementRepository, IMapper mapper)
    {
        _inventoryMovementRepository = inventoryMovementRepository;
        _mapper = mapper;
    }

    public async Task<Result<InventoryMovementRequest?>> Handle(InventoryMovementByIdQuery request, CancellationToken cancellationToken)
    {
        var inventoryMovementDb = await _inventoryMovementRepository.GetByIdAsync(request.MovementId);
        if (inventoryMovementDb == null)
            return Result<InventoryMovementRequest?>.Failure("Movimiento no encontrado", new(), 404);

        return Result<InventoryMovementRequest?>.Success(_mapper.Map<InventoryMovementRequest>(inventoryMovementDb), "Movimiento obtenido con exito");
    }
}
