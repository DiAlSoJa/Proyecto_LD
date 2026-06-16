using System.ComponentModel.DataAnnotations;
using LD.Domain.Common;

namespace LD.Domain.Entities;

public class OperationalTask : AuditableEntity
{
    [Key]
    public int OperationalTaskId { get; set; }

    public int? WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }

    [Required]
    [MaxLength(50)]
    public string Priority { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Activity { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? Photo1Path { get; set; }

    [MaxLength(500)]
    public string? Photo2Path { get; set; }

    [MaxLength(500)]
    public string? Photo3Path { get; set; }

    [MaxLength(500)]
    public string? Photo4Path { get; set; }

    [MaxLength(1000)]
    public string? ResolutionObservations { get; set; }

    [MaxLength(500)]
    public string? ResolvedPhoto1Path { get; set; }

    [MaxLength(500)]
    public string? ResolvedPhoto2Path { get; set; }

    [MaxLength(500)]
    public string? ResolvedPhoto3Path { get; set; }

    [MaxLength(500)]
    public string? ResolvedPhoto4Path { get; set; }

    public bool Completed { get; set; }

    public DateTime? CompletedAt { get; set; }

    [MaxLength(150)]
    public string? CompletedBy { get; set; }
}
