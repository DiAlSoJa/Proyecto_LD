using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;
using CyclicInventoryEntity = LD.Domain.Entities.CyclicInventory;
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

            if (request.Detalles.Count > 0)
            {
                SyncDetalles(inventario, request);
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

    private static void SyncDetalles(CyclicInventoryEntity inventario, InventarioCiclicoRequest request)
    {
        foreach (var detalleRequest in request.Detalles)
        {
            var detalle = inventario.Details.FirstOrDefault(x =>
                x.CyclicInventoryDetailId == detalleRequest.InventarioCiclicoDetalleId);

            if (detalle is null)
            {
                detalle = inventario.Details.FirstOrDefault(x => x.LocationId == detalleRequest.LocationId);
            }

            if (detalle is null)
            {
                detalle = new CyclicInventoryDetailEntity
                {
                    CyclicInventoryId = request.InventarioCiclicoId,
                    LocationId = detalleRequest.LocationId
                };
                inventario.Details.Add(detalle);
            }

            detalle.LocationId = detalleRequest.LocationId;
            detalle.TakeNumber = detalleRequest.TakeNumber <= 0
                ? (detalle.TakeNumber <= 0 ? 1 : detalle.TakeNumber)
                : detalleRequest.TakeNumber;
            detalle.Counted = detalleRequest.Tomada;
            detalle.TheoreticalQty = detalleRequest.Teorico;
            detalle.PhysicalQty = detalleRequest.Fisico;
            detalle.SameLocationQty = detalleRequest.MismaUbicacion;
            detalle.AnotherLocationQty = detalleRequest.EnOtraUbicacion;
            detalle.FirstCountResult = detalleRequest.ResultadoPrimeraToma;
            detalle.SecondCountResult = detalleRequest.ResultadoSegundaToma;
            detalle.ThirdCountResult = detalleRequest.ResultadoTerceraToma;
            detalle.FourthCountResult = detalleRequest.ResultadoCuartaToma;
            detalle.FinalResult = detalleRequest.ResultadoFinal;
            detalle.PartNumber = detalleRequest.PartNumber;
            detalle.Scanned = detalleRequest.Escaneado;
        }
    }
}
