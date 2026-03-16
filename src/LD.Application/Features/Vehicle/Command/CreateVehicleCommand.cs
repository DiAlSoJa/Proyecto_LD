
using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Vehicule.Comands;

public class CreateVehicleCommand : VechicleRequest, IRequest<Result<string>>
{

}


public class CreateVehiculeCommandHandler : IRequestHandler<CreateVehicleCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Vehicle> _vehicleRepository;
    private readonly IMapper _mapper;
    public CreateVehiculeCommandHandler(IRepository<LD.Domain.Entities.Vehicle> vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _vehicleRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.Vehicle>(request));
            return result ? Result<string>.Success("Vehículo creado con exito", "") : Result<string>.Failure("Hubo un error al crear el vehículo", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el vehículo", new List<string> { ex.Message });
        }
    }
}



