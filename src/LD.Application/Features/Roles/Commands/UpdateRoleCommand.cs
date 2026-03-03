using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Roles.Commands;

public class UpdateRoleCommand : RoleRequest, IRequest<Result<string>>
{
    public string RoleId { get; set; }
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result<string>>
{

    private readonly RoleManager<IdentityRole> _roleManager;
    public UpdateRoleCommandHandler(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Result<string>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.RoleId);
        if(role is null) return Result<string>.Failure($"No se pudo actualizar el role con id {request.RoleId}", new(), 401);

        role.Name = request.RoleName;
        var response = await _roleManager.UpdateAsync(role);

        if (!response.Succeeded)
        {
            return Result<string>.Failure($"No se pudo actualizar el usuario con id {request.RoleId}",new(),401);
        }
        return Result<string>.Success("usuario actualizado con exito", "usuario actualizado con exito");
    }
}