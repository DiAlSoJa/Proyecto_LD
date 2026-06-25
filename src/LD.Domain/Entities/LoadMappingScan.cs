using LD.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities;

public class LoadMappingScan : AuditableEntity
{
    [Key]
    public int LoadMappingScanId { get; set; }

    [Required]
    public int LoadMappingId { get; set; }

    public int? KittingReceiptDetailId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Side { get; set; } = string.Empty;

    [MaxLength(100)]
    public string StandardId { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Result { get; set; } = string.Empty;

    public bool IsSuccess { get; set; }

    [MaxLength(100)]
    public string Kitting { get; set; } = string.Empty;

    [MaxLength(100)]
    public string PartNumber { get; set; } = string.Empty;

    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;

    public decimal? Quantity { get; set; }

    [MaxLength(50)]
    public string LotNumber { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Message { get; set; } = string.Empty;

    public DateTime ScannedAt { get; set; }

    public LoadMapping? LoadMapping { get; set; }

    public KittingIssueDetail? KittingIssueDetail { get; set; }
}
