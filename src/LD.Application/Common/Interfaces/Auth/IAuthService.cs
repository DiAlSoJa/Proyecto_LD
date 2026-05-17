using LD.Contracts.Responses;

namespace LD.Application.Common.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse?> Login(string username, string password);
        Task<LoginResponse?> RefreshToken(string refreshToken);
    }
}
