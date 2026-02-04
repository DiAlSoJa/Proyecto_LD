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

namespace LD.Application.Features.Comands;

public class CreateLocationCommand : IRequest<string>
{

}


public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, string>
{
    private readonly IRepository<Location> _locationRepository;
    public CreateLocationCommandHandler(IRepository<Location> locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<string> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var result = await _locationRepository.CreateAsync(new Location
        {
   
        });
        return result ? "Cliente creado con exito" : "Hubo un error al crear el cliente";
    }
}
