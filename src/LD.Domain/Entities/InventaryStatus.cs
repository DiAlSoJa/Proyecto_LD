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
        [Required]
        [MaxLength(20)]
        public string InventoryStatusIdS { get; set; } = string.Empty;   // A, C, D, E...

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;  // DISPONIBLE, CUARENTENA, etc.

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public Client? Client { get; set; }
        public Project? Project { get; set; }

        public bool IsAvailable { get; set; } // Impacta stock disponible

    }
}
