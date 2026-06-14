using System.ComponentModel.DataAnnotations;
using LD.Domain.Common;
using LD.Domain.Enums;

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

    public OperationalTaskStatus Status { get; set; } = OperationalTaskStatus.NoAsignada;

    // Quién tiene la tarea asignada actualmente (nullable: null = sin asignar)
    [MaxLength(450)]
    public string? AssignedToUserId { get; set; }

    public DateTime? AssignedAt { get; set; }

    public bool Completed { get; set; }

    public DateTime? CompletedAt { get; set; }

    // Nombre de texto libre tal como lo envía la app (legacy + display)
    [MaxLength(150)]
    public string? CompletedByName { get; set; }

    // FK al usuario de Identity que completó la tarea
    [MaxLength(450)]
    public string? CompletedByUserId { get; set; }
}
