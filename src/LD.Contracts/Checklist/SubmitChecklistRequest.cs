namespace LD.Contracts.Checklist;

public class SubmitChecklistRequest
{
    public int EquipmentId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Turno { get; set; } = string.Empty;
    public decimal? Horometro { get; set; }
    public string? Observaciones { get; set; }
    public List<ChecklistAnswerDto> Answers { get; set; } = new();
    public List<ChecklistDefectMarkDto> DefectMarks { get; set; } = new();
    // Fotos subidas previamente; solo se mandan las rutas relativas devueltas por la API
    public List<ChecklistPhotoDto> Photos { get; set; } = new();
}
