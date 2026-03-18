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
    public string? RoleId { get; set; }
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result<string>>
{
    private readonly IApplicationUserManager _userManager;
    public UpdateRoleCommandHandler( IApplicationUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<string>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _userManager.UpdateRoleAsync(request.RoleId, request);
            if (!response)
            {
                return Result<string>.Failure($"Hubo un error ",new List<string> { "No se pudo actualizar el usuario con id {request.RoleId}" },401);
            }
            return Result<string>.Success("usuario actualizado con exito", "usuario actualizado con exito");

        }catch (Exception ex) {
            return Result<string>.Failure($"Hubo un error", new List<string> { ex.Message }, 400);

        }
    }
}