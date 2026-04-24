using System.ComponentModel;

namespace LD.Contracts.EquipmentSupplier;

public class EquipmentSupplierDto
{
    [DisplayName("Id")]
    public int EquipmentSupplierId { get; set; }

    [DisplayName("Proveedor")]
    public string EquipmentSupplierName { get; set; } = string.Empty;
}
