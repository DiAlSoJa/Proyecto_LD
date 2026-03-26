using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Dimensioner : AuditableEntity
    {

        [Key]
        [Required]
        [MaxLength(20)]
        public string DimensionerId { get; set; }   // ST, SD, 

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public decimal? Weight { get; set; }




    }
}
