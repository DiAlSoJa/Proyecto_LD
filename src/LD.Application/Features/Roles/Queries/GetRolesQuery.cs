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

public class GetRolesQuery : IRequest<Result<List<RolePermissionDto>>>
{

}

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result<List<RolePermissionDto>>>
{


    private readonly IApplicationUserManager _userManager;
    public GetRolesQueryHandler(IApplicationUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<List<RolePermissionDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _userManager.GetRolesWithPermissionsAsync();
        return Result<List<RolePermissionDto>>.Success(roles, "Roles obtenidos exitosamente");
    }
}
