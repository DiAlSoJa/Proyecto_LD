using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
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

public record WarehouseByIdQuery(int WarehouseId)
    : IRequest<Result<WarehouseDto?>>;


public class WarehouseByIdQueryHandler : IRequestHandler<WarehouseByIdQuery, Result<WarehouseDto?>>
{
    private readonly IRepository<Warehouse> _warehouseRepository;
    private readonly IMapper _mapper;
    public WarehouseByIdQueryHandler(IRepository<Warehouse> warehouseRepository ,IMapper mapper)
    {
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
    }

    public async Task<Result<WarehouseDto?>> Handle(WarehouseByIdQuery request, CancellationToken cancellationToken)
    {
        var warehouseDb = await _warehouseRepository.GetByIdAsync(request.WarehouseId);
        if (warehouseDb == null) return Result<WarehouseDto?>.Failure("Almacen no encontrado", new(), 404);
        return Result<WarehouseDto?>.Success(_mapper.Map<WarehouseDto>(warehouseDb), "Almacen obtenido con exito");
    }
}
