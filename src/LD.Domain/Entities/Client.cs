using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Client :AuditableEntity
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        [MaxLength(20)]
        public string ClientNumber { get; set; }

        [Required]
        [MaxLength(150)]
        public string CommercialName { get; set; }

        [MaxLength(250)]
        public string CommercialAddress { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Neightbourhoud { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? BusinessName { get; set; } = default!;
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(10)]
        public string ZipCode { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Fax { get; set; } = string.Empty;

        [MaxLength(13)]
        public string? Rfc { get; set; } = default!;
        public bool IsProvider { get; set; } 

        //// Relación 1–1
        //public ClienteFiscal ClienteFiscal { get; set; }


    }

}
