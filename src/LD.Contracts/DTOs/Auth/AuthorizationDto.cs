using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.DTOs.Auth
{
    public class AuthorizationDto
    {
        public string? RoleName { get; set; }
        public List<ModuleAuthorizationDto> Modules { get; set; } = new();
    }
}
