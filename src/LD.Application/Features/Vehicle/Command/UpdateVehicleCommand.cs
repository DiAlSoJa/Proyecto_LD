
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

namespace LD.Application.Features.Vehicle.Comands;

public class UpdateVehicleCommand : VechicleRequest, IRequest<Result<string>>
{

}


public class UpdateVehiculeCommandHandler : IRequestHandler<UpdateVehicleCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Vehicle> _vehicleRepository;
    private readonly IMapper _mapper;
    public UpdateVehiculeCommandHandler(IRepository<LD.Domain.Entities.Vehicle> vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.Plates);
            if (vehicle is null)
                return Result<string>.Failure("No existe el vehículo", new List<string> { "Hubo un error al obtener el vehículo" }, 404);
            _mapper.Map(request, vehicle);

            var result = await _vehicleRepository.UpdateAsync(vehicle);
            return result ? Result<string>.Success("Vehículo actualizado con exito", "") : Result<string>.Failure("Hubo un error al actualizar el vehículo", new List<string> { "No se encontro el vehículo" });

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el vehículo", new List<string> { ex.Message });
        }
    }
}

