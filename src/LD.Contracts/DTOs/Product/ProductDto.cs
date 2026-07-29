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

        [DisplayName("Activo")]
        public bool IsActive { get; set; }

        [DisplayName("Categoría")]
        public string Categoria { get; set; } = string.Empty;

        [DisplayName("Familia")]
        public string Familia { get; set; } = string.Empty;     

        [DisplayName("Tipo de almacenamiento")]
        public string TipoDeAlmacenamiento { get; set; }

       

        [DisplayName("Unidad mínima")]
        public string UnidadMinima { get; set; }

        [DisplayName("Unidad medida")]
        public string UnidadMedia { get; set; }

        [DisplayName("Unidad máxima")]
        public string UnidadMaxima { get; set; }

        [DisplayName("Paquete estándar")]
        public string PaqueteEstandar { get; set; }

        [DisplayName("Solicitar número de lote")]
        public bool SolicitarNumeroLote { get; set; }

        [DisplayName("Solicitar fecha de caducidad")]
        public bool SolicitarFechaCaducidad { get; set; }

        [DisplayName("Solicitar número de pedimento")]
        public bool SolicitarPedimento { get; set; }

        [DisplayName("Solicitar tipo de cambio")]
        public bool SolicitarTipoCambio { get; set; }

        [DisplayName("Solicitar orden de compra")]
        public bool SolicitarOrdenCompra { get; set; }

        [DisplayName("Solicitar referencia")]
        public bool SolicitarReferencia { get; set; }

        [DisplayName("Alto")]
        public decimal Alto { get; set; }

        [DisplayName("Ancho")]
        public decimal Ancho { get; set; }

        [DisplayName("Largo")]
        public decimal Largo { get; set; }

        [DisplayName("Peso")]
        public decimal Peso { get; set; }

        [DisplayName("Dimension")]
        public string Dimension { get; set; }
    }


}
