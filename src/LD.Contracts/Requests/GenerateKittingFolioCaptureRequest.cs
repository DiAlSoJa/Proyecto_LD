namespace LD.Contracts.Requests;

public class GenerateKittingFolioCaptureRequest
{
    public int ClientId { get; set; }
    public int ProjectId { get; set; }
    public string SourceFileName { get; set; } = string.Empty;
    public string FileContent { get; set; } = string.Empty;
}
