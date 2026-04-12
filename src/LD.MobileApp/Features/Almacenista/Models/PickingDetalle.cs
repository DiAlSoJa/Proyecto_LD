using System;
using System.Collections.Generic;
using System.Text;

namespace MauiAppLogin.Features.Almacenista.Models
{
    public class PickingDetalle
    {
        public string Pedido { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
}
