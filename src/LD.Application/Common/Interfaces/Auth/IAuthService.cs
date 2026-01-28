using LD.Application.Features.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(string email, string password);
        Task<AuthResponse> Login(string email, string password);
    }
}
