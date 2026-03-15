using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Currency
{
    public class CurrencyDto
    {
        public int CurrencyId { get; set; } 
        public string Moneda { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }


}

