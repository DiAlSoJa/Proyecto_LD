using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using LD.Contracts.User;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LD.Application.Features.Roles.Queries;

public class GetRolesQuery : IRequest<Result<List<IdentityRole>>>
{

}

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result<List<IdentityRole>>>
{

    private readonly RoleManager<IdentityRole> _roleManager;

    public GetRolesQueryHandler(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Result<List<IdentityRole>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles =  _roleManager.Roles.ToList();
        return Result<List<IdentityRole>>.Success(roles, "Roles obtenidos exitosamente");
    }
}
