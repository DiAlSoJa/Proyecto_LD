using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;
using CyclicInventoryEntity = LD.Domain.Entities.CyclicInventory;
using CyclicInventoryDetailEntity = LD.Domain.Entities.CyclicInventoryDetail;

namespace LD.Application.Features.CyclicInventory.Commands;

public class CreateCyclicInventoryCommand : InventarioCiclicoRequest, IRequest<Result<string>>
{
}

public class CreateCyclicInventoryCommandHandler : IRequestHandler<CreateCyclicInventoryCommand, Result<string>>
{
    private readonly IRepository<CyclicInventoryEntity> _repository;
    private readonly IMapper _mapper;

    public CreateCyclicInventoryCommandHandler(IRepository<CyclicInventoryEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateCyclicInventoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var inventario = _mapper.Map<CyclicInventoryEntity>(request);
            inventario.Details = BuildDetalles(request);

            var result = await _repository.CreateAsync(inventario);
            return result
                ? Result<string>.Success("Inventario ciclico creado con exito", "")
                : Result<string>.Failure("Hubo un error al crear el inventario ciclico", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el inventario ciclico", new List<string> { ex.Message });
        }
    }

    private static List<CyclicInventoryDetailEntity> BuildDetalles(InventarioCiclicoRequest request)
    {
        if (request.Detalles.Count > 0)
        {
            return request.Detalles
                .Select(x => new CyclicInventoryDetailEntity
                {
                    LocationId = x.LocationId,
                    TakeNumber = x.TakeNumber <= 0 ? 1 : x.TakeNumber,
                    Counted = x.Tomada,
                    TheoreticalQty = x.Teorico,
                    PhysicalQty = x.Fisico,
                    SameLocationQty = x.MismaUbicacion,
                    AnotherLocationQty = x.EnOtraUbicacion,
                    FirstCountResult = x.ResultadoPrimeraToma,
                    SecondCountResult = x.ResultadoSegundaToma,
                    ThirdCountResult = x.ResultadoTerceraToma,
                    FourthCountResult = x.ResultadoCuartaToma,
                    FinalResult = x.ResultadoFinal,
                    PartNumber = x.PartNumber,
                    Scanned = x.Escaneado
                })
                .ToList();
        }

        return request.LocationIds
            .Distinct()
            .Select(locationId => new CyclicInventoryDetailEntity { LocationId = locationId, TakeNumber = 1 })
            .ToList();
    }
}
