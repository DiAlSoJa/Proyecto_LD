using System;
using System.Collections.Generic;
using System.Text;

namespace MauiAppLogin.Features.Inventario.Models
{
    public class InventoryGroup
    {
        public string Cantidad { get; set; }
        public string Folio { get; set; } = string.Empty;
        public List<InventoryDetail> Detalles { get; set; } = new();
    }
}
