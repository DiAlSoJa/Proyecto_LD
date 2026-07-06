namespace MauiAppLogin.Features.Almacenista.Models;

public class ReceptionDetailItem
{
    public int AsnDetailId { get; set; }
    public int PalletNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string StandardId { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public string QuantityText { get; set; } = string.Empty;
    public string Ubicacion { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string InstructionText { get; set; } = string.Empty;
}
