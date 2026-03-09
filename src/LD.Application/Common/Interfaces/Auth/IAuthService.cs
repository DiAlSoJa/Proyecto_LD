using LD.Application.Common.Models;
using LD.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse?> Login(string username, string password);
    }
}
