using AutoMapper;
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

public class CreateRoleCommand :RoleRequest, IRequest<Result<string>>
{

}

public class CreateRoleCommandHandler
    : IRequestHandler<CreateRoleCommand, Result<string>>
{

    private readonly IApplicationUserManager _userManager;
    public CreateRoleCommandHandler(IApplicationUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _userManager.CreateRoleAsync( request);
            if (!response)
            {
                return Result<string>.Failure($"Hubo un error ", new List<string> { "No se pudo crear el rol con id {request.RoleId}" }, 401);
            }
            return Result<string>.Success("rol creado con exito", "rol creado  con exito");

        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"Hubo un error", new List<string> { ex.Message }, 400);

        }
    }
}