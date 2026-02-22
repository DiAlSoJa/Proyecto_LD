using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Project;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Projects.Queries;

public class ProjectQuery : IRequest<Result<List<ProjectDto?>>>
{

}
public class ProjectQueryHandler : IRequestHandler<ProjectQuery, Result<List<ProjectDto?>>>
{

    private readonly IRepository<Project> _projectRepository;
    private readonly IMapper _mapper;
    public ProjectQueryHandler(IRepository<Project> projectRepository,IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }
    public async Task<Result<List<ProjectDto?>>> Handle(ProjectQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetManyAsync();
        var projectDtos = _mapper.Map<List<ProjectDto>>(projects);
        return Result<List<ProjectDto?>>.Success(projectDtos, "Proyectos obtenidos correctamente");
    }
}
