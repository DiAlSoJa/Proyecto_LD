using LD.Contracts.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests;

public class RoleRequest
{
    public string? RoleName { get; set; }


    public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();

}
