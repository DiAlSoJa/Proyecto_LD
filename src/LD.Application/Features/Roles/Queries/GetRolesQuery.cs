using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.User;
using LD.Contracts.User;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LD.Application.Features.Roles.Queries;

public class GetRolesQuery : IRequest<Result<List<RoleDto>>>
{

}

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result<List<RoleDto>>>
{

    //private readonly RoleManager<IdentityRole> _roleManager;

    public GetRolesQueryHandler()
    {
        //_roleManager = roleManager;
    }

    public async Task<Result<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        //var roles =  _roleManager.Roles.ToList();
        return Result<List<RoleDto>>.Success(null, "Roles obtenidos exitosamente");
    }
}
