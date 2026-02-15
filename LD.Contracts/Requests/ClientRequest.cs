using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class ClientRequest
    {
        public string? CommercialName { get; set; } = default!;
        public string? BusinessName { get; set; } = default!;
        public string? Rfc { get; set; } = default!;
        public string? CommercialAddress { get; set; } = default!;
        public string? Phone { get; set; } = default!;
        public string? City { get; set; } = default!;
        public string? PostalCode { get; set; } = default!;
        public bool IsActive { get; set; }
    }
}
