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

public record GetLocationLookupQuery()
    : IRequest<Result<List<DropDownDto>>>;

public record GetLocationByWarehouseLookupQuery(int WarehouseId)
    : IRequest<Result<List<DropDownDto>>>;


public class GetLocationLookupQueryHandler : IRequestHandler<GetLocationLookupQuery, Result<List<DropDownDto>>>
{

    private readonly ILocationRepository _locationRepository;

    public GetLocationLookupQueryHandler(ILocationRepository locationRepository)
    {

        _locationRepository = locationRepository;
    }
    public async Task<Result<List<DropDownDto>>> Handle(GetLocationLookupQuery request, CancellationToken cancellationToken)
    {
  
        var Locations = await _locationRepository.GetLookup();
        return Result<List<DropDownDto>>.Success(Locations, "Lookups obtenidos con exito");
    }
}

public class GetLocationByWarehouseLookupQueryHandler : IRequestHandler<GetLocationByWarehouseLookupQuery, Result<List<DropDownDto>>>
{
    private readonly ILocationRepository _locationRepository;

    public GetLocationByWarehouseLookupQueryHandler(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<Result<List<DropDownDto>>> Handle(GetLocationByWarehouseLookupQuery request, CancellationToken cancellationToken)
    {
        var locations = await _locationRepository.GetManyAsync() ?? new List<Location>();
        var lookup = locations
            .Where(x => x.WarehouseId == request.WarehouseId)
            .OrderBy(x => x.LocationName)
            .Select(x => new DropDownDto
            {
                Key = x.LocationId.ToString(),
                Value = x.LocationName
            })
            .ToList();

        return Result<List<DropDownDto>>.Success(lookup, "Lookups obtenidos con exito");
    }
}
