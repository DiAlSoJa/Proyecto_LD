using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    [Table("Modules",Schema ="Auth")]
    public class Module : AuditableEntity
    {
        public int ModuleId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? ModuleName { get; set; }

        public int? ParentModuleId { get; set; }

        public Module? ParentModule { get; set; }

        public ICollection<Module>? Children { get; set; }

        public ICollection<Permission>? Permissions { get; set; }
    }
}
