using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.DTOs;
using LD.Contracts.Location;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Queries;

public record GetProjectLookupQuery()
    : IRequest<Result<List<DropDownDto>>>;
public record ProjectByIdQuery(int locationId)
    : IRequest<Result<ProjectRequest?>>;


public class GetProjectLookupQueryHandler : IRequestHandler<GetProjectLookupQuery, Result<List<DropDownDto>>>
{

    private readonly IProjectRepository _projectRepository;

    public GetProjectLookupQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;

    }
    public async Task<Result<List<DropDownDto>>> Handle(GetProjectLookupQuery request, CancellationToken cancellationToken)
    {
        var Projects = await _projectRepository.GetLookup();

        return Result<List<DropDownDto>>.Success(Projects, "Lookups obtenidos con exito");
    }


}


