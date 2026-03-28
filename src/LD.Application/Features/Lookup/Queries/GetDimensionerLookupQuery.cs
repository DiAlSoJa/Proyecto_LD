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

public record GetDimensionerLookupQuery()
    : IRequest<Result<List<DropDownDto>>>;


public class GetDimensionerLookupQueryHandler : IRequestHandler<GetDimensionerLookupQuery, Result<List<DropDownDto>>>
{

    private readonly IDimensionerRepository _unitRepository;

    public GetDimensionerLookupQueryHandler(IDimensionerRepository unitRepository)
    {
        _unitRepository = unitRepository;

    }
    public async Task<Result<List<DropDownDto>>> Handle(GetDimensionerLookupQuery request, CancellationToken cancellationToken)
    {
        var Unites = await _unitRepository.GetLookup();

        return Result<List<DropDownDto>>.Success(Unites, "Lookups obtenidos con exito");
    }
}

