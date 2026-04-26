using System.ComponentModel.DataAnnotations;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class ChecklistAnswer : AuditableEntity
    {
        [Key]
        public int ChecklistAnswerId { get; set; }

        public int ChecklistId { get; set; }
        public Checklist? Checklist { get; set; }

        public int QuestionId { get; set; }

        [MaxLength(500)]
        public string QuestionTextSnapshot { get; set; } = string.Empty;

        [MaxLength(500)]
        public string AnswerText { get; set; } = string.Empty;

        // true=Sí/OK, false=No/Defecto, null=N/A
        public bool? IsOk { get; set; }
    }
}
