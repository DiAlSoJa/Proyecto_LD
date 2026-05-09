using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.CyclicInventory.Queries;

public record CyclicInventoryByIdQuery(int CyclicInventoryId)
    : IRequest<Result<InventarioCiclicoRequest?>>;

public class CyclicInventoryByIdQueryHandler : IRequestHandler<CyclicInventoryByIdQuery, Result<InventarioCiclicoRequest?>>
{
    private readonly IInventarioCiclicoRepository _repository;
    private readonly IMapper _mapper;

    public CyclicInventoryByIdQueryHandler(IInventarioCiclicoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<InventarioCiclicoRequest?>> Handle(CyclicInventoryByIdQuery request, CancellationToken cancellationToken)
    {
        var inventario = await _repository.GetByIdWithRelationsAsync(request.CyclicInventoryId);
        if (inventario is null)
        {
            return Result<InventarioCiclicoRequest?>.Failure("Inventario ciclico no encontrado", new(), 404);
        }

        var result = _mapper.Map<InventarioCiclicoRequest>(inventario);
        result.LocationIds = inventario.Details.Select(x => x.LocationId).ToList();

        return Result<InventarioCiclicoRequest?>.Success(result, "Inventario ciclico obtenido con exito");
    }
}
