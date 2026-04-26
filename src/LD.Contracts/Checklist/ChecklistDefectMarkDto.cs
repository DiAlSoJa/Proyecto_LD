namespace LD.Contracts.Checklist;

public class ChecklistDefectMarkDto
{
    // "Left" | "Right"
    public string Side { get; set; } = "Left";
    // Coordenadas relativas 0..1
    public decimal XPercent { get; set; }
    public decimal YPercent { get; set; }
    public string? Note { get; set; }
}
