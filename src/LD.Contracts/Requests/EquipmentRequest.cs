namespace LD.Contracts.Requests;

public class EquipmentRequest
{
    public int EquipmentId { get; set; }
    public string EquipmentName { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public decimal? Hourmeter { get; set; }
    public bool IsOperative { get; set; }
    public int EquipmentTypeId { get; set; }
    public int WarehouseId { get; set; }
    public int EquipmentSupplierId { get; set; }
    public string Turn1 { get; set; } = string.Empty;
    public string Turn2 { get; set; } = string.Empty;
    public string Turn3 { get; set; } = string.Empty;
    public string ImagePathLeft { get; set; } = string.Empty;
    public string ImagePathRight { get; set; } = string.Empty;
}
