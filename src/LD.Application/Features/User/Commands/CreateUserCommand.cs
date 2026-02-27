


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

public class CreateUserCommand :UserRequest, IRequest<Result<string>>
{

}

public class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, Result<string>>
{

    private readonly IAuthService _authService;
    private readonly IApplicationUserManager _userManager;
    public CreateUserCommandHandler(IAuthService authService,IApplicationUserManager applicationUserManager)
    {
        _authService = authService;
        _userManager = applicationUserManager;
    }

    public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var response = await _userManager.CreateUserAsync(request);
        
        if (response)
        {
            return Result<string>.Success("Todo bien","Usuario creado con exito");
        }
        return Result<string>.Failure("No se pudo crear el usuario", new());
    }
}