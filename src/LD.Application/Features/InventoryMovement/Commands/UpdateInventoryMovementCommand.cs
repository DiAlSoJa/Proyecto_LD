using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.InventoryMovement.Commands;

public class UpdateInventoryMovementCommand : InventoryMovementRequest, IRequest<Result<string>>
{
}

public class UpdateInventoryMovementCommandHandler : IRequestHandler<UpdateInventoryMovementCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.InventoryMovement> _inventoryMovementRepository;
    private readonly IMapper _mapper;

    public UpdateInventoryMovementCommandHandler(IRepository<LD.Domain.Entities.InventoryMovement> inventoryMovementRepository, IMapper mapper)
    {
        _inventoryMovementRepository = inventoryMovementRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateInventoryMovementCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var inventoryMovement = await _inventoryMovementRepository.GetByIdAsync(request.MovementId);
            if (inventoryMovement is null)
                return Result<string>.Failure("No existe el movimiento", new List<string> { "Hubo un error al obtener el movimiento" }, 404);

            _mapper.Map(request, inventoryMovement);

            var result = await _inventoryMovementRepository.UpdateAsync(inventoryMovement);
            return result
                ? Result<string>.Success("Movimiento actualizado con exito", "")
                : Result<string>.Failure("Hubo un error al actualizar el movimiento", new List<string> { "No se encontro el movimiento" });
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el movimiento", new List<string> { ex.Message });
        }
    }
}
