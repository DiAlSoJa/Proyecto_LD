using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class DireccionEntrega : AuditableEntity
    {
        [Key]
        public int DireccionEntregaId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int ProyectoId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Contacto { get; set; }

        [Required]
        [MaxLength(250)]
        public string Direccion { get; set; }

        [MaxLength(100)]
        public string Colonia { get; set; }

        [MaxLength(100)]
        public string Ciudad { get; set; }

        [MaxLength(20)]
        public string Telefono { get; set; }

        [MaxLength(10)]
        public string CodigoPostal { get; set; }

        public bool Activo { get; set; }

        
    }
}
