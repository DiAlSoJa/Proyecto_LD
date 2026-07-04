namespace LD.Contracts.DamageReports;

public class DamageReportDto
{
    public int DamageReportId { get; set; }
    public int? AvailableInventoryId { get; set; }
    public int? StandardId { get; set; }
    public string? StandardIdCode { get; set; }
    public int? ProductId { get; set; }
    public int? ClientId { get; set; }
    public int? ProjectId { get; set; }
    public int? WarehouseId { get; set; }
    public int? LocationId { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? CurrentStatus { get; set; }
    public decimal? ReceivedQuantity { get; set; }
    public decimal? AvailableQuantity { get; set; }
    public string? Warehouse { get; set; }
    public string? Project { get; set; }
    public string? Client { get; set; }
    public string? Asn { get; set; }
    public DateTime? ReceptionDate { get; set; }
    public string? InventoryState { get; set; }
    public string DamageType { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public string? DamageReportCode { get; set; }
    public string? Comments { get; set; }
    public string? Photo1Path { get; set; }
    public string? Photo2Path { get; set; }
    public string? Photo3Path { get; set; }
    public string? Photo4Path { get; set; }
    public DateTime ReportDate { get; set; }
    public string? CreatedByUserId { get; set; }
    public string? CreatedByUserName { get; set; }
    public string? ReportedByUserId { get; set; }
    public string? ReportedByName { get; set; }
}
