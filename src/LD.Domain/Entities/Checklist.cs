using System.ComponentModel.DataAnnotations;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class Checklist : AuditableEntity
    {
        [Key]
        public int ChecklistId { get; set; }

        public int EquipmentId { get; set; }
        public Equipment? Equipment { get; set; }

        // Snapshot del tipo para filtrar Resumen Baterías sin joins adicionales
        public int EquipmentTypeId { get; set; }

        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Turno { get; set; } = string.Empty;
        public decimal? Horometro { get; set; }

        [MaxLength(2000)]
        public string? Observaciones { get; set; }

        public ICollection<ChecklistAnswer> Answers { get; set; } = new List<ChecklistAnswer>();
        public ICollection<ChecklistPhoto> Photos { get; set; } = new List<ChecklistPhoto>();
        public ICollection<ChecklistDefectMark> DefectMarks { get; set; } = new List<ChecklistDefectMark>();
    }
}
