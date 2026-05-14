using LD.Contracts.Equipment;

namespace LD.Contracts.Checklist;

public class ChecklistDailyStatusDto
{
    public bool HasAssignedEquipment { get; set; }
    public bool HasCompletedToday { get; set; }
    public DateTime? LastChecklistAt { get; set; }
    public EquipmentDto? Equipment { get; set; }
}
