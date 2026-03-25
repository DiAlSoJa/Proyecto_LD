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
    public record GetCategoryLookupQuery(int clientId, int projectId)
        : IRequest<Result<List<DropDownDto>>>;


    public class GetCategoryLookupQueryHandler : IRequestHandler<GetCategoryLookupQuery, Result<List<DropDownDto>>>
    {

        private readonly ICategoryRepository _projectRepository;

        public GetCategoryLookupQueryHandler(ICategoryRepository projectRepository)
        {
            _projectRepository = projectRepository;

        }
        public async Task<Result<List<DropDownDto>>> Handle(GetCategoryLookupQuery request, CancellationToken cancellationToken)
        {
            var Projects = await _projectRepository.GetCategoryByClientAsync(request.clientId, request.projectId);

            return Result<List<DropDownDto>>.Success(Projects, "Lookups obtenidos con exito");
        }


    }
}


