namespace LD.Contracts.AvailableInventory;

public class ChangeInventoryLocationRequest
{
    public int StandardId { get; set; }
    public List<int> StandardIds { get; set; } = new();
    public string UbicacionDestino { get; set; } = string.Empty;
    public int? KittingReceiptDetailId { get; set; }
    public int? KittingId { get; set; }
}
