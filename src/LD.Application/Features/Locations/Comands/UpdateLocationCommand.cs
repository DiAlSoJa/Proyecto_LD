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
            var result = await _locationRepository.CreateAsync(_mapper.Map<Location>(request));
            return result ? Result<string>.Success("Ubicacion creado con exito", "") : Result<string>.Failure("Hubo un error al crear el Ubicacion", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el Ubicacion", new ErrorResponse());
        }
    }
}
