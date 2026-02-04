using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Warehouses.Queries;

public class WarehouseQuery : IRequest<List<Warehouse>?>
{

}
public class WarehouseQueryHandler : IRequestHandler<WarehouseQuery, List<Warehouse>?>
{
    private readonly IRepository<Warehouse> _warehouseRepository;
    public WarehouseQueryHandler(IRepository<Warehouse> warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<List<Warehouse>?> Handle(WarehouseQuery request, CancellationToken cancellationToken)
    {

        return await _warehouseRepository.GetManyAsync();
    }
}
