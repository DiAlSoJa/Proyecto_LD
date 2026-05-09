namespace LD.Contracts.InventarioCiclico
{
    public class CyclicInventoryDto
    {
        public int InventarioCiclicoId { get; set; }
        public DateTime Fecha { get; set; }
        public int WarehouseId { get; set; }
        public string Almacen { get; set; } = string.Empty;
        public string AuditorUserId { get; set; } = string.Empty;
        public string Auditor { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
        public DateTime? FechaTerminado { get; set; }
        public List<CyclicInventoryDetailDto> Detalles { get; set; } = [];
    }
}
