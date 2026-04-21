namespace LD.Contracts.Requests;

public class EquipmentQuestionRequest
{
    public int EquipmentQuestionDetId { get; set; }
    public int EquipmentTypeId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string? OptionAnswerText { get; set; }
    public bool IsYesNo { get; set; }
}
