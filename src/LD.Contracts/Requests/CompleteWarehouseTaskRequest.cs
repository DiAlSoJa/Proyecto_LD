using System.ComponentModel.DataAnnotations;

namespace LD.Contracts.Requests;

public class CompleteWarehouseTaskRequest
{
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

    [MaxLength(150)]
    public string? CompletedByName { get; set; }
}
