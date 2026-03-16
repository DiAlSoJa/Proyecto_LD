using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.DTOs.User
{
    public class RolePermissionDto
    {
        public string? Id { get; set; }
        public string? RoleName { get; set; }

        public List<PermissionDto>? Permissions { get; set; }

    }
}
