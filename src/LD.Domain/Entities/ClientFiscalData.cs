using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class ClientFiscalData :AuditableEntity
    {
        public int ClientFiscalDataId { get; set; }

        public int ClientId { get; set; }
        //razon social
        public string BusinessName { get; set; }
        public string Rfc { get; set; }
        public string FiscalAddress { get; set; }

        [MaxLength(100)]
        public string Neightbourhoud { get; set; } = string.Empty;
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(10)]
        public string ZipCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Phone { get; set; } = string.Empty;

        public Client Client { get; set; }


    }

}
