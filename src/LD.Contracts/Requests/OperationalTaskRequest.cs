using System.ComponentModel.DataAnnotations;

namespace LD.Contracts.Requests;

public class OperationalTaskRequest
{
    public int OperationalTaskId { get; set; }

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
}
