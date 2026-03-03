using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using LD.Contracts.User;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LD.Application.Features.Roles.Queries;


public record GetRoleByIdQuery(string RoleId) : IRequest<Result<IdentityRole?>>;
public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Result<IdentityRole?>>
{

    private readonly RoleManager<IdentityRole> _roleManager;

    public GetRoleByIdQueryHandler(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Result<IdentityRole?>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role =  await _roleManager.FindByIdAsync(request.RoleId);

        if (role == null)
            return Result<IdentityRole?>.Failure("No se pudo encontrar el role",new());


        return Result<IdentityRole?>.Success(role, "Role encontrado exitosamente");
    }
}
