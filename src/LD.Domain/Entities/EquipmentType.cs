using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class EquipmentType : AuditableEntity
    {
        [Key]
        public int EquipmentTypeId   { get; set; }

        [Required] 
        public string EquipmentName { get; set; }

        public bool IsBattery {  get; set; }

    }
}
