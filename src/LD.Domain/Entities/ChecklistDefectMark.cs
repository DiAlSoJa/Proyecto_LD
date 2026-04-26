using System.ComponentModel.DataAnnotations;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class ChecklistDefectMark : AuditableEntity
    {
        [Key]
        public int ChecklistDefectMarkId { get; set; }

        public int ChecklistId { get; set; }
        public Checklist? Checklist { get; set; }

        // "Left" | "Right"
        [MaxLength(10)]
        public string Side { get; set; } = "Left";

        // Coordenadas relativas 0..1 (porcentaje sobre la imagen base)
        public decimal XPercent { get; set; }
        public decimal YPercent { get; set; }

        [MaxLength(200)]
        public string? Note { get; set; }
    }
}
