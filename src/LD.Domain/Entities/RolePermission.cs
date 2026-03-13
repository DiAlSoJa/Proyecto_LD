using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    [Table("RolePermissions",Schema ="Auth")]
    public class RolePermission : AuditableEntity
    {
        public string RoleId { get; set; } = default!;

        public int PermissionId { get; set; }

        public Permission Permission { get; set; } = default!;
    }
}
