


using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<Result<string>>
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, Result<string>>
{

    private readonly IAuthService _authService;
    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var auth = await _authService.Login(request.Username, request.Password);
        
        if (!auth.Success)
        {
            return Result<string>.Failure(auth.Message,new(),401);
        }
        return Result<string>.Success(auth.Data.ToString(), auth.Message);
    }
}