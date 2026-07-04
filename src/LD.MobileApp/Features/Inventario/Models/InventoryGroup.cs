using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace MauiAppLogin.Features.Inventario.Models
{
    public class InventoryGroup : ObservableObject
    {
        private bool isExpanded;

        public int InventarioCiclicoId { get; set; }
        public int WarehouseId { get; set; }
        public string AuditorUserId { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public DateTime? FechaTerminado { get; set; }
        public string Almacen { get; set; } = string.Empty;
        public string Auditor { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
        public int Completados { get; set; }
        public int Escaneados { get; set; }
        public bool IsExpanded
        {
            get => isExpanded;
            set => SetProperty(ref isExpanded, value);
        }

        public string Cantidad { get; set; } = string.Empty;
        public string Folio { get; set; } = string.Empty;
        public List<InventoryDetail> Detalles { get; set; } = new();
    }
}
