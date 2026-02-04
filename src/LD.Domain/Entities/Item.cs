using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Item :AuditableEntity
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(20)]
        public string ItemName { get; set; }


    }

}
