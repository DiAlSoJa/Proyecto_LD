using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Warehouse : AuditableEntity
    {
        [Key]
        public int AlmacenId { get; set; }

        [Required]
        [MaxLength(20)]
        public string NumeroAlmacen { get; set; }   // ALMACEN B1, AT_B2, etc.

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(250)]
        public string Domicilio { get; set; }

        [MaxLength(100)]
        public string Colonia { get; set; }

        [MaxLength(100)]
        public string Ciudad { get; set; }

        [MaxLength(10)]
        public string CodigoPostal { get; set; }

        public bool Activo { get; set; }

        public bool Produccion { get; set; }
    }
}
