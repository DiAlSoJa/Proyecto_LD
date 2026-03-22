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
    public class Category : AuditableEntity
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string CategoryIdS { get; set; }   // A, B, C...

        [Required]
        [MaxLength(150)]
        public string Description { get; set; }

        public int Frecuency { get; set; } 

        public Client? Client { get; set; }
        public Project? Project { get; set; }

    }
}
