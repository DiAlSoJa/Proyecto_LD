using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Category : AuditableEntity
    {
        [Key]
        public int CategoriaProyectoId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int ProyectoId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Clave { get; set; }   // A, B, C...

        [Required]
        [MaxLength(150)]
        public string Descripcion { get; set; }

        [Required]
        public int Frecuencia { get; set; }

        public bool Activo { get; set; }

        // Navegación
        //public Cliente Cliente { get; set; }
        //public Proyecto Proyecto { get; set; }
    }
}
