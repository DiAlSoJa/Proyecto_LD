using System.ComponentModel;

namespace LD.Contracts.TruckType;

public class TruckTypeDto
{
    [DisplayName("Id")]
    public int TruckTypeId { get; set; }

    [DisplayName("Tipo")]
    public string Name { get; set; } = string.Empty;

    [DisplayName("Tiene caja")]
    public bool TieneCaja { get; set; }
}
