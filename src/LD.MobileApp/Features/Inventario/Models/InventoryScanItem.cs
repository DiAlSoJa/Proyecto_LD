namespace MauiAppLogin.Features.Inventario.Models
{
    public class InventoryScanItem
    {
        public int CyclicInventoryScanId { get; set; }
        public int Numero { get; set; }
        public string StandardId { get; set; } = string.Empty;
        public string Hora { get; set; } = string.Empty;
    }
}
