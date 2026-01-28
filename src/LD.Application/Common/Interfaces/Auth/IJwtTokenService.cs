using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Interfaces.Auth
{
    public interface IJwtTokenService
    {
        string GenerateToken(IdentityUser user);
    }
}
