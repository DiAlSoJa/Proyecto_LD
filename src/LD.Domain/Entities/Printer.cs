using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Printer : AuditableEntity
    {
        [Key]
        public int PrinterId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }     // Impresora

        [Required]
        [MaxLength(50)]
        public string Addres { get; set; }  // IP o nombre de red

    }
}
