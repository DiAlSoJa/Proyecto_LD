namespace LD.Contracts.Checklist;

public class ChecklistAnswerDto
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string AnswerText { get; set; } = string.Empty;
    public bool? IsOk { get; set; }
}
