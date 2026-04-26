using System.ComponentModel.DataAnnotations;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class ChecklistPhoto : AuditableEntity
    {
        [Key]
        public int ChecklistPhotoId { get; set; }

        public int ChecklistId { get; set; }
        public Checklist? Checklist { get; set; }

        [MaxLength(500)]
        public string RelativePath { get; set; } = string.Empty;

        // "Left" | "Right" | "Custom"
        [MaxLength(20)]
        public string Side { get; set; } = "Custom";

        public int Order { get; set; }
    }
}
