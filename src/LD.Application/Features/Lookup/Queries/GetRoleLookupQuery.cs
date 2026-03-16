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

public record GetRoleLookupQuery()
    : IRequest<Result<List<DropDownDto>>>;


public class GetRoleLookupQueryHandler : IRequestHandler<GetRoleLookupQuery, Result<List<DropDownDto>>>
{

    private readonly IApplicationUserManager _applicationUserManager;

    public GetRoleLookupQueryHandler(IApplicationUserManager applicationUserManager)
    {
        _applicationUserManager = applicationUserManager;
    }
    public async Task<Result<List<DropDownDto>>> Handle(GetRoleLookupQuery request, CancellationToken cancellationToken)
    {
        var lookups = await _applicationUserManager.GetRoleLookupAsync();

        return Result<List<DropDownDto>>.Success(lookups, "Lookups obtenidos con exito");
    }
}
