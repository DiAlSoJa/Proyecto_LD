namespace LD.Contracts.Checklist;

public class ChecklistPhotoDto
{
    public int ChecklistPhotoId { get; set; }
    public string RelativePath { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    // "Left" | "Right" | "Custom"
    public string Side { get; set; } = "Custom";
    public int Order { get; set; }
}
