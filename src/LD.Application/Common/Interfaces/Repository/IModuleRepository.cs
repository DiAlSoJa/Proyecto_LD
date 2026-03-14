using LD.Contracts.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Interfaces.Repository
{
    public interface IModuleRepository
    {
        Task<List<ModuleAuthorizationDto>> GetModules();
    }
}
