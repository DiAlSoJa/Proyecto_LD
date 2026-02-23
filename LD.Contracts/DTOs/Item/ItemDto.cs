using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Item
{
    public class ItemDto
    {
        public int ItemId { get; set; }
        public int ClienteId { get; set; }
        public string Cliente { get; set; } = string.Empty;

        public int ProyectoId { get; set; }
        public string Proyecto { get; set; } = string.Empty;

        public string NumeroParte { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public bool Fifo { get; set; }

        public bool Lifo { get; set; }

        public bool ManejaNumeroLote { get; set; }

        public bool ManejaFechaCaducidad { get; set; }

        public int UnidadMinima { get; set; }

        public int UnidadMedia { get; set; }

        public int UnidadMaxima { get; set; }

        public bool SolicitarNumeroLote { get; set; }

        public bool SolicitarFechaCaducidad { get; set; }
    }


}
