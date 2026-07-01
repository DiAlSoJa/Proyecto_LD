using System.ComponentModel.DataAnnotations;
using LD.Domain.Common;

namespace LD.Domain.Entities;

public class CyclicInventoryScan : AuditableEntity
{
    [Key]
    public int CyclicInventoryScanId { get; set; }

    [Required]
    public int CyclicInventoryId { get; set; }

    [Required]
    public int CyclicInventoryDetailId { get; set; }

    [Required]
    public int LocationId { get; set; }

    [Required]
    [MaxLength(100)]
    public string StandardId { get; set; } = string.Empty;

    public DateTime ScannedAt { get; set; }
    public bool IsCorrectScan { get; set; }

    [MaxLength(250)]
    public string? CurrentLocation { get; set; }
    public int? CurrentLocationId { get; set; }

    public bool IsInAnotherLocation { get; set; }
    public bool InventoryNotAvailable { get; set; }

    public CyclicInventory? CyclicInventory { get; set; }
    public CyclicInventoryDetail? CyclicInventoryDetail { get; set; }
    public Location? Location { get; set; }
}
