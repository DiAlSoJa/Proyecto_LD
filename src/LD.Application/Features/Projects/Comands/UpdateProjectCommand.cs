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

public class UpdateProjectCommand :ProjectRequest, IRequest<Result<string>>
{

}


public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result<string>>
{

    private readonly IRepository<Project> _projectRepository;
    private readonly IMapper _mapper;
    public UpdateProjectCommandHandler(IRepository<Project> projectRepository,IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }
    public async Task<Result<string>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _projectRepository.CreateAsync(_mapper.Map<Project>(request));
            return result ? Result<string>.Success("Projecto creado con exito", "") : Result<string>.Failure("Hubo un error al crear el Projecto", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el Projecto", new List<string> { ex.Message });
        }
       
    }
}
