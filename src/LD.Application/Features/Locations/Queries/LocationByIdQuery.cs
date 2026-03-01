using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Location;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Queries;

public record LocationByIdQuery(int locationId)
    : IRequest<Result<LocationDto?>>;


public class LocationByIdQueryHandler : IRequestHandler<LocationByIdQuery, Result<LocationDto?>>
{

    private readonly IRepository<Location> _locationRepository;
    private readonly IMapper _mapper;
    public LocationByIdQueryHandler(IRepository<Location> locationRepository, IMapper mapper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
    }
    public async Task<Result<LocationDto?>> Handle(LocationByIdQuery request, CancellationToken cancellationToken)
    {
        var locationDb = await _locationRepository.GetByIdAsync(request.locationId);
        if (locationDb == null) return Result<LocationDto?>.Failure("Ubicacion no encontrado", new(), 404);
        return Result<LocationDto?>.Success(_mapper.Map<LocationDto>(locationDb), "Ubicacion obtenido con exito");
    }
}
