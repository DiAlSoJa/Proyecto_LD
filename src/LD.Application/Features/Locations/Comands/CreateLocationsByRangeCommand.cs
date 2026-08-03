
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;


namespace LD.Application.Features.Comands;

public class CreateLocationsByRangeCommand : LocationRequest, IRequest<Result<string>>
{

}


public class CreateLocationsByRangeCommandHandler : IRequestHandler<CreateLocationsByRangeCommand, Result<string>>
{
    private readonly ILocationRepository _locationRepository;

    public CreateLocationsByRangeCommandHandler(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<Result<string>> Handle(CreateLocationsByRangeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.WarehouseId <= 0)
                return Result<string>.Failure("WarehouseId debe ser mayor a 0.", new());

            if (string.IsNullOrWhiteSpace(request.Rack))
                return Result<string>.Failure("El Rack es obligatorio.", new());

            if (request.FromW <= 0)
                return Result<string>.Failure("Desde debe ser mayor a 0.", new());

            if (request.ToW < request.FromW)
                return Result<string>.Failure("Hasta no puede ser menor que Desde.", new());

            if (request.Leves <= 0)
                return Result<string>.Failure("Niveles debe ser mayor a 0.", new());

            if (request.Leves > 26)
                return Result<string>.Failure("Niveles no puede ser mayor a 26.", new());

            string rack = request.Rack.Trim().ToUpper();

            var locationsToCreate = new List<Location>();

            for (int position = (int) request.FromW; position <= request.ToW; position++)
            {
                string positionText = position.ToString("D2");

                for (int nivel = 0; nivel < request.Leves; nivel++)
                {
                    string levelText = ((char)('A' + nivel)).ToString();
                    string locationName = $"{rack}{positionText}{levelText}";

                    locationsToCreate.Add(new Location
                    {
                        WarehouseId = request.WarehouseId,
                        Rack = rack,
                        Position = positionText,
                        Level = levelText,
                        LocationName = locationName,
                        Aisle = string.IsNullOrWhiteSpace(request.Aisle) ? null : request.Aisle.Trim(),
                        IsActive = request.IsActive,
                        IsFiscal = request.IsFiscal,
                        HasControlledTemperature = request.HasControlledTemperature,
                        Height = request.Height,
                        Width   = request.Width,
                        Depth = request.Depth,
                        IsRack = request.IsRack,
                        IsCompartidoType = request.IsCompartidoType,
                        IsGeneral = request.IsGeneral,
                        IsCuarentena = request.IsCuarentena,
                        IsEmbarque = request.IsEmbarque,
                        IsCompartido = request.IsCompartido,
                        IsReciboYEmbarque = request.IsReciboYEmbarque,
                        IsDoble = request.IsDoble,
                        IsSencillo = request.IsSencillo,
                        HasPaso = request.HasPaso,
                        HasCortina = request.HasCortina,
                        Ocupado = request.Ocupado,
                        Placas = request.Placas,


                    });
                }
            }

            var duplicatedInRequest = locationsToCreate
                .GroupBy(x => x.LocationName)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicatedInRequest.Any())
            {
                return Result<string>.Failure(
                    "La solicitud contiene ubicaciones duplicadas.",
                    duplicatedInRequest
                );
            }

            var locationNames = locationsToCreate
                .Select(x => x.LocationName)
                .ToList();

            var duplicatesInDatabase = await _locationRepository
                .GetExistingLocationNamesAsync(request.WarehouseId, locationNames);

            if (duplicatesInDatabase.Any())
            {
                return Result<string>.Failure(
                    $"Ya existen {duplicatesInDatabase.Count} ubicaciones registradas.",
                    duplicatesInDatabase.OrderBy(x => x).ToList()
                );
            }

            var result = await _locationRepository.CreateRangeAsync(locationsToCreate);

            return result
                ? Result<string>.Success($"Se crearon {locationsToCreate.Count} ubicaciones correctamente.", "")
                : Result<string>.Failure("Hubo un error al crear las ubicaciones.", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(
                "Hubo un error al crear las ubicaciones.",
                new List<string> { ex.Message }
            );
        }
    }
}
