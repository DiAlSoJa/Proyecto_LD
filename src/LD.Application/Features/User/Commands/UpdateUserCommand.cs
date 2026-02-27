


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

public class UpdateUserCommand :UserRequest, IRequest<Result<string>>
{

}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<string>>
{

    private readonly IAuthService _authService;
    private readonly IApplicationUserManager _applicationUserManager;
    public UpdateUserCommandHandler(IAuthService authService,IApplicationUserManager applicationUserManager)
    {
        _authService = authService;
        _applicationUserManager = applicationUserManager;
    }

    public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var auth = await _applicationUserManager.UpdateAsync(request);

        if (!auth)
        {
            return Result<string>.Failure($"No se pudo actualizar el usuario con id {request.UserId}",new(),401);
        }
        return Result<string>.Success("usuario actualizado con exito", "usuario actualizado con exito");
    }
}