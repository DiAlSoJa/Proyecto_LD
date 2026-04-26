namespace LD.Contracts.Checklist;

public class ChecklistDetailDto
{
    public int ChecklistId { get; set; }
    public int EquipmentId { get; set; }
    public string EquipmentName { get; set; } = string.Empty;
    public int EquipmentTypeId { get; set; }
    public string EquipmentTypeName { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }
    public string? Brand { get; set; }
    public bool IsOperative { get; set; }
    public string? SupplierName { get; set; }
    public decimal? Hourmeter { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Turno { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int TotalDefects { get; set; }
    public string? Observaciones { get; set; }

    public List<ChecklistAnswerDto> Answers { get; set; } = new();
    public List<ChecklistDefectMarkDto> DefectMarks { get; set; } = new();
    public List<ChecklistPhotoDto> Photos { get; set; } = new();
}
