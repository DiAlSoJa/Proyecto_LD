using LD.Contracts.InventarioCiclico;

namespace LD.Contracts.Requests
{
    public class InventarioCiclicoRequest
    {
        public int InventarioCiclicoId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Today;
        public string AuditorUserId { get; set; } = string.Empty;
        public string? AuditorNombre { get; set; }
        public int WarehouseId { get; set; }
        public string Estatus { get; set; } = "Abierto";
        public DateTime? FechaTerminado { get; set; }
        public List<int> LocationIds { get; set; } = [];
        public List<CyclicInventoryDetailDto> Detalles { get; set; } = [];
    }
}
