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

public class LocationQuery : IRequest<Result<List<LocationDto>?>>
{

}
public class LocationQueryHandler : IRequestHandler<LocationQuery, Result<List<LocationDto>?>>
{

    private readonly IRepository<Location> _locationRepository;
    private readonly IMapper _mapper;
    public LocationQueryHandler(IRepository<Location> locationRepository,IMapper mapper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
    }
    public async Task<Result<List<LocationDto>?>> Handle(LocationQuery request, CancellationToken cancellationToken)
    {
        var locations = await _locationRepository.GetManyAsync();
        var locationDtos = _mapper.Map<List<LocationDto>>(locations);
        return Result<List<LocationDto>?>.Success(locationDtos, "Ubicaciones obtenidos correctamente");
    }
}
