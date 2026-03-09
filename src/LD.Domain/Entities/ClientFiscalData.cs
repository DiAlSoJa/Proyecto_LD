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

        [Required]
        public int ClientId { get; set; }
        //razon social
        [MaxLength(150)]
        public string? BusinessName { get; set; } = string.Empty;
        [MaxLength(20)]
        public string? Rfc { get; set; } = string.Empty;
        [MaxLength(250)]
        public string? FiscalAddress { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Neightbourhoud { get; set; } = string.Empty;
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(10)]
        public string ZipCode { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Phone { get; set; } = string.Empty;

        public Client? Client { get; set; }


    }

}
