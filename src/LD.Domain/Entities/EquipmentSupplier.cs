using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public  class EquipmentSupplier : AuditableEntity
    {
        [Key]
        public int EquipmentSupplierId { get; set; }
        [Required]
        public string  EquipmentSupplierName { get; set; }


    }
}
