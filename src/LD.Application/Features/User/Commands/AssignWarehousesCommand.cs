


using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.User.Commands;

public class AssignWarehousesCommand : UserWarehouseRequest, IRequest<Result<string>>
{
    public string? UserId { get; set; }
}

public class AssignWarehousesCommandHandler
    : IRequestHandler<AssignWarehousesCommand, Result<string>>
{
    IApplicationUserManager _userManager;
    public AssignWarehousesCommandHandler(IApplicationUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<string>> Handle(AssignWarehousesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
                return Result<string>.Failure("El userId es requerido", new());

            var result = await _userManager.AssignWarehousesAsync(request.UserId, request.WarehouseIds ?? new());

            if (!result)
                return Result<string>.Failure("No se pudieron asignar los almacenes", new());

            return Result<string>.Success("Almacenes asignados correctamente", "Almacenes asignados correctamente");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error", new List<string> { ex.Message }, 400);
        }
    }
}