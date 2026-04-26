namespace LD.Contracts.Checklist;

public class GetChecklistsQueryRequest
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    // null = todos los tipos; se usa para filtrar por Batería en ResumenBaterias
    public int? EquipmentTypeId { get; set; }
    public int? EquipmentId { get; set; }
}
