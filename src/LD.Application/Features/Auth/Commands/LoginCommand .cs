


using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<Result<LoginResponse>>
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{

    private readonly IAuthService _authService;
    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var auth = await _authService.Login(request.Username!, request.Password!);
        
            if (auth is null)
            {
                return Result<LoginResponse>.Failure("Hubo un error al iniciar sesion", new List<string> { "Credenciales invalidas" }, 401);
            }
            return Result<LoginResponse>.Success(auth, "Login exitoso");

        } catch (Exception ex)
        {
            return Result<LoginResponse>.Failure("Hubo un error al iniciar sesion", new List<string> { ex.Message});
        }
   
    }
}