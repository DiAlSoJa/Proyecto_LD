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
        [Required]
        [MaxLength(20)]
        public string UnitIdS { get; set; }   // KG, PZA, PAL, CJ

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }


    }
}
