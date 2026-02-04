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

namespace LD.Application.Features.Projects.Queries;

public record ProjectByIdQuery(int WarehouseId)
    : IRequest<Project?>;


public class ProjectByIdQueryHandler : IRequestHandler<ProjectByIdQuery, Project?>
{

    private readonly IRepository<Project> _projectRepository;
    public ProjectByIdQueryHandler(IRepository<Project> projectRepository)
    {
        _projectRepository = projectRepository;
    }
    public async Task<Project?> Handle(ProjectByIdQuery request, CancellationToken cancellationToken)
    {
        return await _projectRepository.GetByIdAsync(request.WarehouseId);
    }
}
