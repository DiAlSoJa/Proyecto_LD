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
        public int ImpresoraEtiquetaId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }     // Impresora

        [Required]
        [MaxLength(50)]
        public string Direccion { get; set; }  // IP o nombre de red

        public bool Activo { get; set; }
    }
}
