using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Location;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Queries;

public record LocationByIdQuery(int locationId)
    : IRequest<Result<LocationRequest?>>;


public class LocationByIdQueryHandler : IRequestHandler<LocationByIdQuery, Result<LocationRequest?>>
{

    private readonly IRepository<Location> _locationRepository;
    private readonly IMapper _mapper;
    public LocationByIdQueryHandler(IRepository<Location> locationRepository, IMapper mapper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
    }
    public async Task<Result<LocationRequest?>> Handle(LocationByIdQuery request, CancellationToken cancellationToken)
    {
        var locationDb = await _locationRepository.GetByIdAsync(request.locationId);
        if (locationDb == null) return Result<LocationRequest?>.Failure("Ubicacion no encontrado", new(), 404);
        return Result<LocationRequest?>.Success(_mapper.Map<LocationRequest>(locationDb), "Ubicacion obtenido con exito");
    }
}
