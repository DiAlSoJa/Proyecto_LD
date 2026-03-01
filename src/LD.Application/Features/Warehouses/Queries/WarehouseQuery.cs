using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Warehouse;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Warehouses.Queries;

public class WarehouseQuery : IRequest<Result< List<WarehouseDto>?>>
{

}
public class WarehouseQueryHandler : IRequestHandler<WarehouseQuery, Result<List<WarehouseDto>?>>
{
    private readonly IRepository<Warehouse> _warehouseRepository;
    private readonly IMapper _mapper;
    public WarehouseQueryHandler(IRepository<Warehouse> warehouseRepository,IMapper mapper)
    {
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<WarehouseDto>?>> Handle(WarehouseQuery request, CancellationToken cancellationToken)
    {
        var warehouse = await _warehouseRepository.GetManyAsync();
        var warehouseDtos = _mapper.Map<List<WarehouseDto>>(warehouse);
        return Result<List<WarehouseDto>?>.Success(warehouseDtos, "Almacenes obtenidos correctamente");
    }
}
