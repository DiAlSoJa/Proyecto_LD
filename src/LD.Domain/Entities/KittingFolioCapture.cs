using LD.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities;

public class KittingFolioCapture : AuditableEntity
{
    [Key]
    [Required]
    public int KittingFolioCaptureId { get; set; }

    [Required]
    public int KittingId { get; set; }

    public int? KittingDetailId { get; set; }

    [MaxLength(260)]
    public string? SourceFileName { get; set; }

    public int SourceLineNumber { get; set; }

    [MaxLength(50)]
    public string? GuideNumber { get; set; }

    [MaxLength(50)]
    public string? InvoiceNumber { get; set; }

    [Required]
    [MaxLength(100)]
    public string PartNumber { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; set; }

    public decimal Quantity { get; set; }

    [MaxLength(50)]
    public string? LotNumber { get; set; }

    [MaxLength(30)]
    public string? SourceStatus { get; set; }

    public Kitting? Kitting { get; set; }

    public KittingDetail? KittingDetail { get; set; }
}
