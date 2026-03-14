using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Interfaces.Auth
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(string userId, string permission);
    }
}
