using LD.Domain.Common;
using LD.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities;

public class WarehouseTask : AuditableEntity
{
    [Key]
    public int WarehouseTaskId { get; set; }

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

    /// <summary>
    /// Orden manual para priorizar tareas. Menor = mayor prioridad.
    /// El listado se ordena por este campo; la lógica de reordenamiento aún no está implementada.
    /// </summary>
    public int OrderIndex { get; set; }

    public WarehouseTaskStatus Status { get; set; } = WarehouseTaskStatus.NoAsignada;

    [MaxLength(450)]
    public string? AssignedToUserId { get; set; }

    public DateTime? AssignedAt { get; set; }

    [MaxLength(450)]
    public string? CompletedByUserId { get; set; }

    [MaxLength(150)]
    public string? CompletedByName { get; set; }

    public DateTime? CompletedAt { get; set; }
}
