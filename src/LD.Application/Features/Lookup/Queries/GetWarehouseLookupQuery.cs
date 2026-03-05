using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.DTOs;
using LD.Contracts.Location;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Queries;

public record GetWarehouseLookupQuery()
    : IRequest<Result<List<DropDownDto>>>;


public class GetWarehouseLookupQueryHandler : IRequestHandler<GetWarehouseLookupQuery, Result<List<DropDownDto>>>
{

    private readonly IWarehouseRepository _warehouseRepository;

    public GetWarehouseLookupQueryHandler(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;

    }
    public async Task<Result<List<DropDownDto>>> Handle(GetWarehouseLookupQuery request, CancellationToken cancellationToken)
    {
       var Warehouses = await _warehouseRepository.GetLookup();

        return Result<List<DropDownDto>>.Success(Warehouses, "Lookups obtenidos con exito");
    }
}
