namespace LD.Contracts.AvailableInventory;

public class ChangeInventoryLocationRequest
{
    public int StandardId { get; set; }
    public List<int> StandardIds { get; set; } = new();
    public string UbicacionDestino { get; set; } = string.Empty;
}
