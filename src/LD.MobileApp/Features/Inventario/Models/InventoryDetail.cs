using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace MauiAppLogin.Features.Inventario.Models
{
    public class InventoryDetail : ObservableObject
    {
        private bool tomada;
        private bool escaneado;
        private string color = string.Empty;

        public int InventarioCiclicoDetalleId { get; set; }
        public int LocationId { get; set; }
        public int TakeNumber { get; set; } = 1;
        public string Pedido { get; set; } = string.Empty;
        public string? PartNumber { get; set; }
        public DateTime Fecha { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string Color
        {
            get => color;
            set => SetProperty(ref color, value);
        }
        public decimal? Teorico { get; set; }
        public decimal? Fisico { get; set; }
        public decimal? MismaUbicacion { get; set; }
        public decimal? EnOtraUbicacion { get; set; }
        public string? ResultadoPrimeraToma { get; set; }
        public string? ResultadoSegundaToma { get; set; }
        public string? ResultadoTerceraToma { get; set; }
        public string? ResultadoCuartaToma { get; set; }
        public string? ResultadoFinal { get; set; }

        public bool Tomada
        {
            get => tomada;
            set
            {
                if (SetProperty(ref tomada, value))
                {
                    OnPropertyChanged(nameof(DetailSummary));
                    UpdateColor();
                }
            }
        }

        public bool Escaneado
        {
            get => escaneado;
            set
            {
                if (SetProperty(ref escaneado, value))
                {
                    OnPropertyChanged(nameof(DetailSummary));
                    UpdateColor();
                }
            }
        }

        public string DetailSummary =>
            $"Tomada: {(Tomada ? "Si" : "No")} | Escaneado: {(Escaneado ? "Si" : "No")}";

        private void UpdateColor()
        {
            Color = Tomada ? "#D2F2B6" : Escaneado ? "#FDE68A" : "#FFFFFF";
        }
    }
}
