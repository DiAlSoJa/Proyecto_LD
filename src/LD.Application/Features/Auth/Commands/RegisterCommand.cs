using LD.Application.Common.Interfaces.Auth;
using LD.Application.Features.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Auth.Commands;
public class RegisterCommand : IRequest<AuthResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponse> Handle( RegisterCommand request, CancellationToken cancellationToken)
    {
        return await _authService.Register(request.Email, request.Password);
    }
}
