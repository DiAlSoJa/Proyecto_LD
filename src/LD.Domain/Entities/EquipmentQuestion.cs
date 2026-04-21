using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class EquipmentQuestion : AuditableEntity
    {
        [Key]
        public int EquipmentQuestionId { get; set; }       
        public int EquipmentTypeId { get; set; }
        public EquipmentType? EquipmentType { get; set; } = null;   

    }

}
