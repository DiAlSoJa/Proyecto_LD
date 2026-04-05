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

namespace LD.Application.Features.Projects.Comands;

public class CreateProjectCommand :ProjectRequest, IRequest<Result<string>>
{

}


public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<string>>
{

    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    public CreateProjectCommandHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }
    public async Task<Result<string>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = _mapper.Map<Project>(request);

            foreach (var scanReq in request.ScanConfigurations)
                project.ScanConfigurations.Add(_mapper.Map<ScanConfiguration>(scanReq));

            var result = await _projectRepository.CreateAsync(project);
            return result ? Result<string>.Success("Projecto creado con exito", "") : Result<string>.Failure("Hubo un error al crear el Projecto", new List<string> { "No se pudo encontrar el projecto" });

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el Projecto", new List<string> { ex.Message });
        }

    }
}
