using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Permission : AuditableEntity
    {
        public int PermissionId { get; set; }
        [Required]
        [MaxLength(100)]
        public string? PermissionName { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Key { get; set; }

        public int? ModuleId { get; set; }
        public Module? Module { get; set; }
    }
}
