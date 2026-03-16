
using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Requests;

using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Vehicle.Queries;

public record VehicleByIdQuery(int vehiculeId)
    : IRequest<Result<VechicleRequest?>>;


public class VehicleByIdQueryHandler : IRequestHandler<VehicleByIdQuery, Result<VechicleRequest?>>
{
    private readonly IRepository<LD.Domain.Entities.Vehicle> _vehicleRepository;
    private readonly IMapper _mapper;
    public VehicleByIdQueryHandler(IRepository<LD.Domain.Entities.Vehicle> vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<Result<VechicleRequest?>> Handle(VehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehiculeDb = await _vehicleRepository.GetByIdAsync(request.vehiculeId);
        if (vehiculeDb == null) return Result<VechicleRequest?>.Failure("Vehículo no encontrado", new(), 404);
        return Result<VechicleRequest?>.Success(_mapper.Map<VechicleRequest>(vehiculeDb), "Vehículo obtenido con éxito");
    }
}
