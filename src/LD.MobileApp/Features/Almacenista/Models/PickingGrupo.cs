using System;
using System.Collections.Generic;
using System.Text;

namespace MauiAppLogin.Features.Almacenista.Models
{
    public class PickingGrupo
    {
        public int Cantidad { get; set; }
        public string Folio { get; set; } = string.Empty;
        public List<PickingDetalle> Detalles { get; set; } = new();
    }
}
