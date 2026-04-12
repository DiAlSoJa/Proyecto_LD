using System;
using System.Collections.Generic;
using System.Text;

namespace MauiAppLogin.Features.Inventario.Models
{
    public class InventoryDetail
    {
        public string Pedido { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }

}
