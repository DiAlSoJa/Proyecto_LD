namespace MauiAppLogin.Features.Almacenista.Models;

public class PickingKittingIssueItem
{
    public int KittingReceiptDetailId { get; set; }
    public int KittingDetailId { get; set; }
    public string StandardId { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public string QuantityText { get; set; } = string.Empty;
    public string LocationText { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string InstructionText { get; set; } = string.Empty;
}
