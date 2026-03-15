using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class CurrencyRequest
    {


        public int? CurrencyId { get; set; }
        public string? Clave { get; set; }   // MXN, USD, EUR


        public string? Description { get; set; }



    }


}

