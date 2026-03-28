using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class DimensionerRequest
    {
        public string DimensionerId { get; set; }   // ST, SD, 


        public string Description { get; set; }

        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public decimal? Weight { get; set; }



    }


}

