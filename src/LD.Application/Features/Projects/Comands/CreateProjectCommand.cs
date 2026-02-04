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

namespace LD.Application.Features.Projects.Comands;

public class CreateProjectCommand : IRequest<string>
{

}


public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, string>
{

    private readonly IRepository<Project> _projectRepository;
    public CreateProjectCommandHandler(IRepository<Project> projectRepository)
    {
        _projectRepository = projectRepository;
    }
    public async Task<string> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var result = await _projectRepository.CreateAsync(new Project
        {
   
        });
        return result ? "Cliente creado con exito" : "Hubo un error al crear el cliente";
    }
}
