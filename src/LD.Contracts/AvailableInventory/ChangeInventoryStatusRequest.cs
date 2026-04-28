namespace LD.Contracts.AvailableInventory;

public class ChangeInventoryStatusRequest
{
    public int StandardId { get; set; }
    public List<int> StandardIds { get; set; } = new();
    public string StatusDestino { get; set; } = string.Empty;
}
