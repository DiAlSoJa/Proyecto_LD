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

namespace LD.Application.Features.Queries;

public class LocationQuery : IRequest<List<Location>?>
{

}
public class LocationQueryHandler : IRequestHandler<LocationQuery, List<Location>?>
{

    private readonly IRepository<Location> _locationRepository;
    public LocationQueryHandler(IRepository<Location> locationRepository)
    {
        _locationRepository = locationRepository;
    }
    public async Task<List<Location>?> Handle(LocationQuery request, CancellationToken cancellationToken)
    {

        return await _locationRepository.GetManyAsync();
    }
}
