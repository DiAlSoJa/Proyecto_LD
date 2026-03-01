using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests.Client
{
    public class ClientRequest
    {
        public string? CommercialName { get; set; } = default!;
        public string? CommercialAddress { get; set; } = default!;
        public string? Neightbourhoud { get; set; } = default!;
        public string? City { get; set; } = default!;
        public string? ZipCode { get; set; } = default!;
        public string? Phone { get; set; } = default!;
        public bool IsProvider { get; set; }
        public bool IsActive { get; set; }

        public ClientFiscalDataRequest? FiscalData { get; set; }
    }
}
