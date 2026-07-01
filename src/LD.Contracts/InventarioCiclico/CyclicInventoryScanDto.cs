namespace LD.Contracts.InventarioCiclico;

public class CyclicInventoryScanDto
{
    public int CyclicInventoryScanId { get; set; }
    public int CyclicInventoryId { get; set; }
    public int CyclicInventoryDetailId { get; set; }
    public int LocationId { get; set; }
    public string Ubicacion { get; set; } = string.Empty;
    public string StandardId { get; set; } = string.Empty;
    public DateTime ScannedAt { get; set; }
    public bool IsCorrectScan { get; set; }
    public string? CurrentLocation { get; set; }
    public int? CurrentLocationId { get; set; }
    public bool IsInAnotherLocation { get; set; }
    public bool InventoryNotAvailable { get; set; }
}
