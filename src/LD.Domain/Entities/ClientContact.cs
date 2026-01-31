using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class ClientContact : AuditableEntity
    {
        [Key]
        public int ContactoId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }

        [MaxLength(100)]
        public string Puesto { get; set; }

        [MaxLength(150)]
        public string Correo { get; set; }

        [MaxLength(20)]
        public string Telefono { get; set; }

        [MaxLength(20)]
        public string Fax { get; set; }

        [MaxLength(500)]
        public string Notas { get; set; }

        public bool Activo { get; set; }

        // Navegación
        public Client Cliente { get; set; }
    }
}
