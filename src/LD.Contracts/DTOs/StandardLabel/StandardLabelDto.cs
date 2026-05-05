namespace LD.Contracts.DTOs.StandardLabel;

public class StandardLabelDto
{
    public int StandarId { get; set; }
    public string StandarIdStr { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public int? ClientId { get; set; }
    public int? ProjectId { get; set; }
    public bool IsAssigned { get; set; }
}
