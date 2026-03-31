using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Dimensioner
{
    public class DimensionerDto
    {
        [DisplayName("Dimensión")]
        public string DimensionerId { get; set; }   // ST, SD, 
        [DisplayName("Descripción")]
        public string Description { get; set; }=String.Empty;
        [DisplayName("Alto")]
        public decimal? Height { get; set; }
        [DisplayName("Ancho")]
        public decimal? Width { get; set; }
        [DisplayName("Largo")]
        public decimal? Length { get; set; }
        [DisplayName("Peso")]
        public decimal? Weight { get; set; }
    }


}

