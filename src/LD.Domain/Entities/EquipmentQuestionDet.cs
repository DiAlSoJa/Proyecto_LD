using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class EquipmentQuestionDet: AuditableEntity
    {
        [Key]
        public int EquipmentQuestionDetId { get; set; }
        public int EquipmentQuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string? OptionAnswerText { get; set; }
        public bool IsYesNo { get; set; }
        public EquipmentQuestion? EquipmentQuestion { get; set; } = null;

    }
}
