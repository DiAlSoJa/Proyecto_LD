using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Application.Features.Queries;
using LD.Contracts.DTOs;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Lookup.Queries
{
    public record GetProjecClienttLookupQuery(int clientId)
        : IRequest<Result<List<DropDownDto>>>;
    public record ProjectByIdQuery(int locationId)
        : IRequest<Result<ProjectRequest?>>;


    public class GetProjectClientLookupQueryHandler : IRequestHandler<GetProjecClienttLookupQuery, Result<List<DropDownDto>>>
    {

        private readonly IProjectRepository _projectRepository;

        public GetProjectClientLookupQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;

        }
        public async Task<Result<List<DropDownDto>>> Handle(GetProjecClienttLookupQuery request, CancellationToken cancellationToken)
        {
            var Projects = await _projectRepository.GetProjectByClientAsync(request.clientId);

            return Result<List<DropDownDto>>.Success(Projects, "Lookups obtenidos con exito");
        }


    }
}


