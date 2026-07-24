using System.Collections.Generic;

namespace LD.Contracts.DTOs.KittingFolioCapture;

public class KittingFolioCapturePreviewDto
{
    public string SourceFileName { get; set; } = string.Empty;

    public string GuideNumber { get; set; } = string.Empty;

    public string InvoiceNumber { get; set; } = string.Empty;

    public int TotalRows { get; set; }

    public int ValidRows { get; set; }

    public int InvalidRows { get; set; }

    public List<KittingFolioCapturePreviewRowDto> Rows { get; set; } = new();
}
