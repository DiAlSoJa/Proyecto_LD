namespace LD.Contracts.EquipmentQuestion;

public class EquipmentQuestionDto
{
    public int EquipmentQuestionId { get; set; }
    public int EquipmentQuestionDetId { get; set; }
    public int EquipmentTypeId { get; set; }
    public string EquipmentTypeName { get; set; } = string.Empty;
    public string QuestionText { get; set; } = string.Empty;
    public string? OptionAnswerText { get; set; }
    public bool IsYesNo { get; set; }
}
