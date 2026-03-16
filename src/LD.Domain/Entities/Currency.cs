using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Currency : AuditableEntity
    {
        [Key]
        [Required]
        [MaxLength(5)]
        public string CurrencyIdS { get; set; }   // MXN, USD, EUR

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }  // PESOS MEXICANOS

    }
}
