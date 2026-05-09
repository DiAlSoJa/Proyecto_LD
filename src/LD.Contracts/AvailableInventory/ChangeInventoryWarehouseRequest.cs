namespace LD.Contracts.AvailableInventory;

public class ChangeInventoryWarehouseRequest
{
    public int StandardId { get; set; }
    public List<int> StandardIds { get; set; } = new();
    public int WarehouseId { get; set; }
    public string UbicacionDestino { get; set; } = string.Empty;
}
