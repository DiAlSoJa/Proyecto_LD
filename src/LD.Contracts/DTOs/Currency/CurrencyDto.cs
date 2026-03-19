using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Currency
{
    public class CurrencyDto
    {
        [DisplayName("Moneda")]
        public string CurrencyIdS { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; } = string.Empty;
    }


}

