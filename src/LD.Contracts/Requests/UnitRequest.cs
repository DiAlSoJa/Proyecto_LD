using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class UnitRequest
    {


        public int? UnitId { get; set; }

        public string? Clave { get; set; }   // KG, PZA, PAL, CJ


        public string? Description { get; set; }



    }


}

