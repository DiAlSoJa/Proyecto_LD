namespace LD.Contracts.Requests;

public class SecurityTaskActionRequest
{
    public string? RealizadaPor { get; set; }
    public string FotoBase64 { get; set; } = string.Empty;
}
