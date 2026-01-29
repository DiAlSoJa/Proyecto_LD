using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class InventaryStatus : AuditableEntity
    {
        [Key]
        public int EstatusInventarioId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Clave { get; set; }   // A, C, D, E...

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }  // DISPONIBLE, CUARENTENA, etc.

        public bool Disponible { get; set; } // Impacta stock disponible

    }
}
