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

public record GetLookupsQuery()
    : IRequest<Result<LookupsDto?>>;


public class GetLookupsQueryHandler : IRequestHandler<GetLookupsQuery, Result<LookupsDto?>>
{

    private readonly IWarehouseRepository _warehouseRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IClientRepository _clientRepository;

    public GetLookupsQueryHandler(IWarehouseRepository warehouseRepository, ILocationRepository locationRepository, IClientRepository clientRepository)
    {
        _warehouseRepository = warehouseRepository;
        _locationRepository = locationRepository;
        _clientRepository = clientRepository;
    }
    public async Task<Result<LookupsDto?>> Handle(GetLookupsQuery request, CancellationToken cancellationToken)
    {
        var lookups = new LookupsDto();
        lookups.Warehouses = await _warehouseRepository.GetLookup();
        lookups.Clients = await _clientRepository.GetLookup();

        lookups.Locations = await _locationRepository.GetLookup();
        return Result<LookupsDto?>.Success(lookups, "Lookups obtenidos con exito");
    }
}
