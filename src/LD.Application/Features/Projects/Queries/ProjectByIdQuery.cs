using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Projects.Queries;

public record ProjectByIdQuery(int projectId)
    : IRequest<Result<ProjectRequest?>>;


public class ProjectByIdQueryHandler : IRequestHandler<ProjectByIdQuery, Result<ProjectRequest?>>
{

    private readonly IRepository<Project> _projectRepository;
    private readonly IMapper _mapper;
    public ProjectByIdQueryHandler(IRepository<Project> projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }
    public async Task<Result<ProjectRequest?>> Handle(ProjectByIdQuery request, CancellationToken cancellationToken)
    {

        var prjectDb = await _projectRepository.GetByIdAsync(request.projectId);
        if (prjectDb == null) return Result<ProjectRequest?>.Failure("Proyecto no encontrado", new(), 404);
        return Result<ProjectRequest?>.Success(_mapper.Map<ProjectRequest>(prjectDb), "Proyecto obtenido con exito");

    }
}
