using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;
using CyclicInventoryDetailEntity = LD.Domain.Entities.CyclicInventoryDetail;

namespace LD.Application.Features.CyclicInventory.Commands;

public class UpdateCyclicInventoryCommand : InventarioCiclicoRequest, IRequest<Result<string>>
{
}

public class UpdateCyclicInventoryCommandHandler : IRequestHandler<UpdateCyclicInventoryCommand, Result<string>>
{
    private readonly IInventarioCiclicoRepository _repository;
    private readonly IMapper _mapper;

    public UpdateCyclicInventoryCommandHandler(IInventarioCiclicoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateCyclicInventoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var inventario = await _repository.GetByIdWithRelationsAsync(request.InventarioCiclicoId);
            if (inventario is null)
            {
                return Result<string>.Failure("Inventario ciclico no encontrado", new(), 404);
            }

            _mapper.Map(request, inventario);

            if (request.Detalles.Count > 0 || request.LocationIds.Count > 0)
            {
                inventario.Details.Clear();
                foreach (var detalle in BuildDetalles(request))
                {
                    inventario.Details.Add(detalle);
                }
            }

            var result = await _repository.UpdateAsync(inventario);
            return result
                ? Result<string>.Success("Inventario ciclico actualizado con exito", "")
                : Result<string>.Failure("Hubo un error al actualizar el inventario ciclico", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el inventario ciclico", new List<string> { ex.Message });
        }
    }

    private static List<CyclicInventoryDetailEntity> BuildDetalles(InventarioCiclicoRequest request)
    {
        if (request.Detalles.Count > 0)
        {
            return request.Detalles
                .Select(x => new CyclicInventoryDetailEntity
                {
                    CyclicInventoryId = request.InventarioCiclicoId,
                    LocationId = x.LocationId,
                    Counted = x.Tomada,
                    TheoreticalQty = x.Teorico,
                    PhysicalQty = x.Fisico,
                    FirstCountResult = x.ResultadoPrimeraToma,
                    SecondCountResult = x.ResultadoSegundaToma,
                    FinalResult = x.ResultadoFinal,
                    PartNumber = x.PartNumber,
                    Scanned = x.Escaneado
                })
                .ToList();
        }

        return request.LocationIds
            .Distinct()
            .Select(locationId => new CyclicInventoryDetailEntity
            {
                CyclicInventoryId = request.InventarioCiclicoId,
                LocationId = locationId
            })
            .ToList();
    }
}
