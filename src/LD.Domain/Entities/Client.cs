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
        public string ComercialName { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string Neightbourhoud { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(10)]
        public string ZipCode { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(20)]
        public string Fax { get; set; }

        public bool Activo { get; set; }

        public bool IsProvider { get; set; }

        //// Relación 1–1
        //public ClienteFiscal ClienteFiscal { get; set; }


    }

}
