namespace LD.Contracts.DTOs.OperationalTasks;

public class OperationalTaskDto
{
    public int OperationalTaskId { get; set; }
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
    public bool Completed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? CompletedBy { get; set; }
}
