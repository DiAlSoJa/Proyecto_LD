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

public record LocationByIdQuery(int WarehouseId)
    : IRequest<Location?>;


public class LocationByIdQueryHandler : IRequestHandler<LocationByIdQuery, Location?>
{

    private readonly IRepository<Location> _locationRepository;
    public LocationByIdQueryHandler(IRepository<Location> locationRepository)
    {
        _locationRepository = locationRepository;
    }
    public async Task<Location?> Handle(LocationByIdQuery request, CancellationToken cancellationToken)
    {
        return await _locationRepository.GetByIdAsync(request.WarehouseId);
    }
}
