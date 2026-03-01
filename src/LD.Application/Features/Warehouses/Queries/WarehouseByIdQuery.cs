using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Requests;
using LD.Contracts.Warehouse;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Warehouses.Queries;

public record WarehouseByIdQuery(int WarehouseId)
    : IRequest<Result<WarehouseRequest?>>;


public class WarehouseByIdQueryHandler : IRequestHandler<WarehouseByIdQuery, Result<WarehouseRequest?>>
{
    private readonly IRepository<Warehouse> _warehouseRepository;
    private readonly IMapper _mapper;
    public WarehouseByIdQueryHandler(IRepository<Warehouse> warehouseRepository ,IMapper mapper)
    {
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
    }

    public async Task<Result<WarehouseRequest?>> Handle(WarehouseByIdQuery request, CancellationToken cancellationToken)
    {
        var warehouseDb = await _warehouseRepository.GetByIdAsync(request.WarehouseId);
        if (warehouseDb == null) return Result<WarehouseRequest?>.Failure("Almacen no encontrado", new(), 404);
        return Result<WarehouseRequest?>.Success(_mapper.Map<WarehouseRequest>(warehouseDb), "Almacen obtenido con exito");
    }
}
