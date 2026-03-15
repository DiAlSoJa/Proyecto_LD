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
        public int CategoryId { get; set; }
      
        //[Required]
        //public int ProyectoId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Clave { get; set; }   // A, B, C...

        [Required]
        [MaxLength(150)]
        public string Description { get; set; }

    }
}
