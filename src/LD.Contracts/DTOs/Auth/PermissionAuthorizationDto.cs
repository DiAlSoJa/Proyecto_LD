using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.DTOs.Auth
{
    public class PermissionAuthorizationDto
    {
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
        public string? Key { get; set; }

    }
}
