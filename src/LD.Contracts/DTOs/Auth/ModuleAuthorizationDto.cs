using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.DTOs.Auth
{
    public class ModuleAuthorizationDto
    {
        public int? ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public List<PermissionAuthorizationDto> Permissions { get; set; } = new();
    }
}
