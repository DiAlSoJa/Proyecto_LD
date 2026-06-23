namespace LD.Contracts.DTOs.Kitting;

public class KittingValidationPhotoDto
{
    public string PhotoKey { get; set; } = string.Empty;
    public int? KittingValidationPhotoId { get; set; }
    public int SortOrder { get; set; }
    public string RelativePath { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsLegacy { get; set; }
}
