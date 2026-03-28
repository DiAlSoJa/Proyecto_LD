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

        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public decimal? Weight { get; set; }
    }


}

