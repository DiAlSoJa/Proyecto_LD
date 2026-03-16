using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Units;
using LD.Contracts.Vehicle;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Vehicle.Queries;

public class VehicleQuery : IRequest<Result<List<VehicleDto>?>>
{

}
public class VehicleQueryHandler : IRequestHandler<VehicleQuery, Result<List<VehicleDto>?>>
{
    private readonly IRepository<LD.Domain.Entities.Vehicle> _vehicleRepository;
    private readonly IMapper _mapper;
    public VehicleQueryHandler(IRepository<LD.Domain.Entities.Vehicle> vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<VehicleDto>?>> Handle(VehicleQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetManyAsync();
        var vehicleDtos = _mapper.Map<List<VehicleDto>>(vehicle);
        return Result<List<VehicleDto>?>.Success(vehicleDtos, "Vehículos obtenidos correctamente");
    }
}
