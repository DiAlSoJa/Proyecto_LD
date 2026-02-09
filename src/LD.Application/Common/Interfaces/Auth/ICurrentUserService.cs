using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Interfaces.Auth
{
    public interface IUserContextService
    {
        string? UserId { get; }
        string? Email { get; }
    }
}
