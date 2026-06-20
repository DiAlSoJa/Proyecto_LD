namespace MauiAppLogin.Features.Almacenista.Models;

public class PickingKittingItem
{
    public int KittingId { get; set; }
    public int IssueCount { get; set; }
    public string KittingCode { get; set; } = string.Empty;
    public string Client { get; set; } = string.Empty;
    public string Project { get; set; } = string.Empty;
    public string InvoiceNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string InstructionText { get; set; } = string.Empty;
}
