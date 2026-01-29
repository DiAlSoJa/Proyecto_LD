using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Location : AuditableEntity
    {
        [Key]
        public int UbicacionId { get; set; }

        [Required]
        public int AlmacenId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Codigo { get; set; }   // Ej: RACK-A01-N01

        public bool Activa { get; set; }

        // Flags operativos
        public bool Fiscal { get; set; }
        public bool TemperaturaControlada { get; set; }

        // Dimensiones (cm)
        public decimal? AltoCm { get; set; }
        public decimal? AnchoCm { get; set; }
        public decimal? ProfundidadCm { get; set; }


     
    }


}
