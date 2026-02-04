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

public class ProjectQuery : IRequest<List<Project>?>
{

}
public class ProjectQueryHandler : IRequestHandler<ProjectQuery, List<Project>?>
{

    private readonly IRepository<Project> _projectRepository;
    public ProjectQueryHandler(IRepository<Project> projectRepository)
    {
        _projectRepository = projectRepository;
    }
    public async Task<List<Project>?> Handle(ProjectQuery request, CancellationToken cancellationToken)
    {

        return await _projectRepository.GetManyAsync();
    }
}
