using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.InventoryMovement.Commands;

public class CreateInventoryMovementCommand : InventoryMovementRequest, IRequest<Result<string>>
{
}

public class CreateInventoryMovementCommandHandler : IRequestHandler<CreateInventoryMovementCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.InventoryMovement> _inventoryMovementRepository;
    private readonly IMapper _mapper;

    public CreateInventoryMovementCommandHandler(IRepository<LD.Domain.Entities.InventoryMovement> inventoryMovementRepository, IMapper mapper)
    {
        _inventoryMovementRepository = inventoryMovementRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateInventoryMovementCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _inventoryMovementRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.InventoryMovement>(request));
            return result
                ? Result<string>.Success("Movimiento creado con exito", "")
                : Result<string>.Failure("Hubo un error al crear el movimiento", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el movimiento", new List<string> { ex.Message });
        }
    }
}
