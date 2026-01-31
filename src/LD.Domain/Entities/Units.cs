using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Units : AuditableEntity
    {
        [Key]
        public int UnitId { get; set; }

        [Required]
        [MaxLength(10)]
        public string Clave { get; set; }   // KG, PZA, PAL, CJ

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }


    }
}
