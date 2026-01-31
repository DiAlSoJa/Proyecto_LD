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
        public int InventoryStatusId { get; set; }

        [Required]
        public int IdClient { get; set; }

        [Required]
        [MaxLength(50)]
        public string Clave { get; set; }   // A, C, D, E...

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; }  // DISPONIBLE, CUARENTENA, etc.

        public bool IsAvailable { get; set; } // Impacta stock disponible

    }
}
