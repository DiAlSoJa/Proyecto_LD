namespace LD.Contracts.DTOs.KittingFolioCapture;

public class KittingFolioCapturePreviewRowDto
{
    public int SourceLineNumber { get; set; }

    public string PartNumber { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public string LotNumber { get; set; } = string.Empty;

    public bool IsValid { get; set; }

    public string ValidationMessage { get; set; } = string.Empty;

    public string StatusText => IsValid ? "Listo" : "Error";
}
