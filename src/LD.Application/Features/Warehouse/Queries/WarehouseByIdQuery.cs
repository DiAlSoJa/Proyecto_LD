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

namespace LD.Application.Features.Auth.Commands;

public record WarehouseByIdQuery(int WarehouseId)
    : IRequest<Warehouse?>;


public class WarehouseByIdQueryHandler : IRequestHandler<WarehouseByIdQuery, Warehouse?>
{
    private readonly IRepository<Warehouse> _warehouseRepository;
    public WarehouseByIdQueryHandler(IRepository<Warehouse> warehouseRepository )
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<Warehouse?> Handle(WarehouseByIdQuery request, CancellationToken cancellationToken)
    {
        return await _warehouseRepository.GetByIdAsync(request.WarehouseId);
    }
}
