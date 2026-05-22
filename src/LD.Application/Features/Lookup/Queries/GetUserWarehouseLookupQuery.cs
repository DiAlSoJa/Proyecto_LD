using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs;
using MediatR;

namespace LD.Application.Features.Lookup.Queries;

public record GetUserWarehouseLookupQuery(string UserId)
    : IRequest<Result<List<DropDownDto>>>;

public class GetUserWarehouseLookupQueryHandler : IRequestHandler<GetUserWarehouseLookupQuery, Result<List<DropDownDto>>>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public GetUserWarehouseLookupQueryHandler(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<Result<List<DropDownDto>>> Handle(GetUserWarehouseLookupQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            return Result<List<DropDownDto>>.Failure("Usuario invalido", new());

        var warehouses = await _warehouseRepository.GetLookupByUserId(request.UserId);
        return Result<List<DropDownDto>>.Success(warehouses, "Almacenes obtenidos con exito");
    }
}
