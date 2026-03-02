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

namespace LD.Application.Features.Comands;

public class UpdateLocationCommand : LocationRequest, IRequest<Result<string>>
{
    public int LocationId { get; set; }
}


public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, Result<string>>
{
    private readonly IRepository<Location> _locationRepository;
    private readonly IMapper _mapper;
    public UpdateLocationCommandHandler(IRepository<Location> locationRepository,IMapper mapper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var location = await _locationRepository.GetByIdAsync(request.LocationId);
            if (location is null)
                return Result<string>.Failure("No existe la ubicacion", new ErrorResponse(), 404);

            _mapper.Map(request, location);


            var updated = await _locationRepository.UpdateAsync(location);

            if (!updated)
                return Result<string>.Failure("Error al actualizar", new ErrorResponse());


            return Result<string>.Success("Ubicacion actualizada", location.LocationId.ToString());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el Ubicacion", new ErrorResponse());
        }
    }
}
