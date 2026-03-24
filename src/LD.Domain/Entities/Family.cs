using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Family : AuditableEntity
    {
        [Key]
        [Required]
        public int FamilyId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FamilyName { get; set; }

        [Required]
        public int? ProjectId { get; set; }

        [Required]
        public int? ClientId { get; set; }       

        public Project? Project { get; set; }
        public Client? Client { get; set; }
    }
}
