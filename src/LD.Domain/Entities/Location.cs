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
        public int LocationId { get; set; }

        //[Required]
        //public int AlmacenId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }   // Ej: RACK-A01-N01


        // Flags operativos
        public bool Fiscal { get; set; }
        public bool TemperaturaControlada { get; set; }

        // Dimensiones (cm)
        public decimal? HeightCm { get; set; }
        public decimal? WeightCm { get; set; }
        public decimal? ProfundidadCm { get; set; }


     
    }


}
