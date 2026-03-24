using LD.Contracts.User;
using LD.Contracts.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.DTOs.User
{
    public class GetUserDto
    {
        public UserDto? User { get; set; }
        public List<PermissionDto>? Permissions { get; set; }
        public List<WarehouseDto>? Warehouse { get; set; }

    }
}
