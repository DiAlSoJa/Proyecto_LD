using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Client
    {
        [Key]
        public int ClienteId { get; set; }

        [Required]
        [MaxLength(20)]
        public string NumeroCliente { get; set; }

        [Required]
        [MaxLength(150)]
        public string NombreComercial { get; set; }

        [MaxLength(250)]
        public string Domicilio { get; set; }

        [MaxLength(100)]
        public string Colonia { get; set; }

        [MaxLength(100)]
        public string Ciudad { get; set; }

        [MaxLength(10)]
        public string CodigoPostal { get; set; }

        [MaxLength(20)]
        public string Telefono { get; set; }

        [MaxLength(20)]
        public string Fax { get; set; }

        public bool Activo { get; set; }

        public bool EsProveedor { get; set; }

        //// Relación 1–1
        //public ClienteFiscal ClienteFiscal { get; set; }

        // Auditoría (muy recomendable)
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }

}
