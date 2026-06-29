namespace LD.Contracts.Requests;

public class CreateLoadMappingScanRequest
{
    public int? KittingReceiptDetailId { get; set; }

    public string Side { get; set; } = string.Empty;

    public string StandardId { get; set; } = string.Empty;

    public string Result { get; set; } = string.Empty;

    public bool IsSuccess { get; set; }

    public string Kitting { get; set; } = string.Empty;

    public string PartNumber { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal? Quantity { get; set; }

    public string LotNumber { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}
