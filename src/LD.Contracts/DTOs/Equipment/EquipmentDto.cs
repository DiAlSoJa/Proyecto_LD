using System.ComponentModel;

namespace LD.Contracts.Equipment;

public class EquipmentDto
{
    [DisplayName("Id")]
    public int EquipmentId { get; set; }

    [DisplayName("Tipo")]
    public int EquipmentTypeId { get; set; }

    [DisplayName("Tipo")]
    public string Tipo { get; set; } = string.Empty;

    [DisplayName("No. de Equipo")]
    public string NoEquipo { get; set; } = string.Empty;

    [DisplayName("Serie")]
    public string Serie { get; set; } = string.Empty;

    [DisplayName("Marca")]
    public string Marca { get; set; } = string.Empty;

    [DisplayName("Horometro")]
    public decimal? Horometro { get; set; }

    [DisplayName("Operativo")]
    public string Operativo { get; set; } = string.Empty;

    [DisplayName("Proveedor")]
    public int EquipmentSupplierId { get; set; }

    [DisplayName("Proveedor")]
    public string Proveedor { get; set; } = string.Empty;

    [DisplayName("Turno 1")]
    public string Turno1 { get; set; } = string.Empty;

    [DisplayName("Turno 2")]
    public string Turno2 { get; set; } = string.Empty;

    [DisplayName("Turno 3")]
    public string Turno3 { get; set; } = string.Empty;
}
