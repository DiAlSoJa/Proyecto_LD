using LD.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities;

public class KittingValidationPhoto : AuditableEntity
{
    [Key]
    public int KittingValidationPhotoId { get; set; }

    public int KittingId { get; set; }
    public Kitting? Kitting { get; set; }

    [Required]
    [MaxLength(500)]
    public string RelativePath { get; set; } = string.Empty;

    public int SortOrder { get; set; }
}
