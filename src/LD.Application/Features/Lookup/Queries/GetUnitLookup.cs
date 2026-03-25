using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.DTOs;
using LD.Contracts.Location;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Queries;

public record GetUnitLookupQuery()
    : IRequest<Result<List<DropDownDto>>>;


public class GetUnitLookupQueryHandler : IRequestHandler<GetUnitLookupQuery, Result<List<DropDownDto>>>
{

    private readonly IUnitRepository _unitRepository;

    public GetUnitLookupQueryHandler(IUnitRepository unitRepository)
    {
        _unitRepository = unitRepository;

    }
    public async Task<Result<List<DropDownDto>>> Handle(GetUnitLookupQuery request, CancellationToken cancellationToken)
    {
        var Unites = await _unitRepository.GetLookup();

        return Result<List<DropDownDto>>.Success(Unites, "Lookups obtenidos con exito");
    }
}

