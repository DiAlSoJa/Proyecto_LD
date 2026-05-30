using System;
using System.Collections.Generic;
using System.Text;

namespace MauiAppLogin.Features.Almacenista.Models
{
    public class ListItemRecep
    {
        public string Titulo { get; set; } = "";
        public string Subtitulo { get; set; } = "";
        public int AsnId { get; set; }
        public string AsnCode { get; set; } = "";
        public string LocationCode { get; set; } = "";
        public int PalletsPorMover { get; set; }
        public string InstructionText { get; set; } = "";
    }
}
