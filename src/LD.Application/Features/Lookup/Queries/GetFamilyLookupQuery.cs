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
    public record GetFamilyLookupQuery(int clientId, int projectId)
        : IRequest<Result<List<DropDownDto>>>;


    public class GetFamilyLookupQueryHandler : IRequestHandler<GetFamilyLookupQuery, Result<List<DropDownDto>>>
    {

        private readonly IFamilyRepository _projectRepository;

        public GetFamilyLookupQueryHandler(IFamilyRepository projectRepository)
        {
            _projectRepository = projectRepository;

        }
        public async Task<Result<List<DropDownDto>>> Handle(GetFamilyLookupQuery request, CancellationToken cancellationToken)
        {
            var Projects = await _projectRepository.GetFamilyByClientAsync(request.clientId, request.projectId);

            return Result<List<DropDownDto>>.Success(Projects, "Lookups obtenidos con exito");
        }


    }
}


