using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Contracts.User;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LD.Application.Features.Roles.Queries;


public record GetRoleByIdQuery(string RoleId) : IRequest<Result<RoleRequest?>>;
public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Result<RoleRequest?>>
{

    //private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IApplicationUserManager _userManager;

    public GetRoleByIdQueryHandler(IApplicationUserManager userManager)
    {
        //_roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<Result<RoleRequest?>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role =  await _userManager.GetRoleByIdAsync(request.RoleId);

        if (role == null)
           return Result<RoleRequest?>.Failure("No se pudo encontrar el role",new List<string>() { "Compruebe El id del rol"});


        return Result<RoleRequest?>.Success(role, "Role encontrado exitosamente");
    }
}
