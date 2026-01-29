using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<AuthResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, AuthResponse>
{

    private readonly IAuthService _authService;
    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(LoginCommand request,CancellationToken cancellationToken)
    {
        return await _authService.Login(request.Email, request.Password);
    }
}