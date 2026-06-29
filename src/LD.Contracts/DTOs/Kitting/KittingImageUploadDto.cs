namespace LD.Contracts.Kitting;

public class KittingImageUploadDto
{
    public string RelativePath { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int PhotoNumber { get; set; }
}
