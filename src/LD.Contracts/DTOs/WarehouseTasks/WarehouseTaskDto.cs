using LD.Contracts.Enums;

namespace LD.Contracts.DTOs.WarehouseTasks;

public class WarehouseTaskDto
{
    public int WarehouseTaskId { get; set; }
    public int? WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public string Priority { get; set; } = string.Empty;
    public string Activity { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Photo1Path { get; set; }
    public string? Photo2Path { get; set; }
    public string? Photo3Path { get; set; }
    public string? Photo4Path { get; set; }
    public string? ResolutionObservations { get; set; }
    public string? ResolvedPhoto1Path { get; set; }
    public string? ResolvedPhoto2Path { get; set; }
    public string? ResolvedPhoto3Path { get; set; }
    public string? ResolvedPhoto4Path { get; set; }
    public WarehouseTaskStatus Status { get; set; }
    public string? AssignedToUserId { get; set; }
    public DateTime? AssignedAt { get; set; }
    public string? CompletedByUserId { get; set; }
    public string? CompletedByName { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
