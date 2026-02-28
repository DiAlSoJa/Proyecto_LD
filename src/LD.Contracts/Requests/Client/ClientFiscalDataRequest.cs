using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests.Client
{
    public class ClientFiscalDataRequest
    {
        public string? BusinessName { get; set; }
        public string? Rfc { get; set; }
        public string? FiscalAddress { get; set; }
        public string? Neightbourhoud { get; set; } 
        public string? City { get; set; } 
        public string? ZipCode { get; set; } 
        public string? Email { get; set; }
        public string? Phone { get; set; }

    }
}
