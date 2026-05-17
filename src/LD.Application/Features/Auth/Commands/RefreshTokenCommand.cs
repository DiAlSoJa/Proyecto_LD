using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using LD.Contracts.Responses;
using MediatR;

namespace LD.Application.Features.Auth.Commands;

public class RefreshTokenCommand : IRequest<Result<LoginResponse>>
{
    public string RefreshToken { get; set; } = null!;
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
{
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var auth = await _authService.RefreshToken(request.RefreshToken);

            if (auth is null)
                return Result<LoginResponse>.Failure("Sesión expirada", new List<string> { "Refresh token inválido o expirado" }, 401);

            return Result<LoginResponse>.Success(auth, "Token renovado");
        }
        catch (Exception ex)
        {
            return Result<LoginResponse>.Failure("Error al renovar sesión", new List<string> { ex.Message }, 401);
        }
    }
}
