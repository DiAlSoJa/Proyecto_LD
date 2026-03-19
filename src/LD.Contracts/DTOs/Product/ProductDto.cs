using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Product
{
    public class ProductDto
    {
        [DisplayName("Id")]
        public int ItemId { get; set; }


        [DisplayName("Cliente")]
        public string Cliente { get; set; } = string.Empty;

        [DisplayName("Proyeto")]
        public string Proyecto { get; set; } = string.Empty;

        [DisplayName("No. parte")]
        public string NumeroParte { get; set; } = string.Empty;

        [DisplayName("Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        [DisplayName("FIFO")]
        public bool Fifo { get; set; }

        [DisplayName("LIFO")]
        public bool Lifo { get; set; }

        [DisplayName("Número de lote")]
        public bool ManejaNumeroLote { get; set; }

        [DisplayName("Fecha de caducidad")]
        public bool ManejaFechaCaducidad { get; set; }

        [DisplayName("Unidad mínima")]
        public int UnidadMinima { get; set; }

        [DisplayName("Unidad medida")]
        public int UnidadMedia { get; set; }

        [DisplayName("Unidad máxima")]
        public int UnidadMaxima { get; set; }

        [DisplayName("Solicitar número de lote")]
        public bool SolicitarNumeroLote { get; set; }

        [DisplayName("Solicitar fecha de caducidad")]
        public bool SolicitarFechaCaducidad { get; set; }
    }


}
