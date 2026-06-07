namespace LD.Contracts.Requests;

public class LocateAsnPalletRequest
{
    public int StandardId { get; set; }
    public string UbicacionDestino { get; set; } = string.Empty;
}
