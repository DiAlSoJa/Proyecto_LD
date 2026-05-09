namespace LD.Contracts.InventarioCiclico
{
    public class CyclicInventoryDetailDto
    {
        public int InventarioCiclicoDetalleId { get; set; }
        public int InventarioCiclicoId { get; set; }
        public int LocationId { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public bool Tomada { get; set; }
        public decimal? Teorico { get; set; }
        public decimal? Fisico { get; set; }
        public string? ResultadoPrimeraToma { get; set; }
        public string? ResultadoSegundaToma { get; set; }
        public string? ResultadoFinal { get; set; }
        public string? PartNumber { get; set; }
        public bool Escaneado { get; set; }
    }
}
