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
        [MaxLength(150)]
        public string? CommercialName { get; set; } = string.Empty;

        [MaxLength(250)]
        public string CommercialAddress { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Neightbourhoud { get; set; } = string.Empty;
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(10)]
        public string ZipCode { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        public bool IsProvider { get; set; } 

        public ClientFiscalData? ClientFiscalData { get; set; }


    }

}
