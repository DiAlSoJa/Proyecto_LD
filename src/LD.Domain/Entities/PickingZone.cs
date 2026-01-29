using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class PickingZone : AuditableEntity
    {
        [Key]
        public int ZonaPickingId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int ProyectoId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int UbicacionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string NumeroParte { get; set; }

        [Required]
        [MaxLength(200)]
        public string Descripcion { get; set; }

        public int Minimo { get; set; }

        public bool Particionar { get; set; }

        public bool Activo { get; set; }

        // Navegación
        //public Cliente Cliente { get; set; }
        //public Proyecto Proyecto { get; set; }
        //public Producto Producto { get; set; }
        //public Ubicacion Ubicacion { get; set; }
    }

}
