using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Location
{
    public class LocationDto
    {
        [DisplayName("Id")]
        public int LocationId { get; set; }
        public int WarehouseId { get; set; }
        public bool Activo { get; set; }

        [DisplayName("Almacén")]
        public string Almacen { get; set; } = string.Empty;

        public string Rack { get; set; } = string.Empty;

        public string Nivel { get; set; } = string.Empty;

        [DisplayName("Posición")]
        public string Posicion { get; set; } = string.Empty;


        [DisplayName("Ubicación")]
        public string Ubicacion { get; set; } = string.Empty;

        [DisplayName("Dimensión")]
        public string Dimension { get; set; } = string.Empty;

        [DisplayName("Es fiscal")]
        public bool EsFiscal { get; set; }

        [DisplayName("Temperatura controlada")]
        public bool ControlTemperatura { get; set; }

        [DisplayName("Rack")]
        public bool EsRack { get; set; }

        [DisplayName("General")]
        public bool EsGeneral { get; set; }

        [DisplayName("Cuarentena")]
        public bool EsCuarentena { get; set; }

        [DisplayName("Embarque")]
        public bool EsEmbarque { get; set; }

        [DisplayName("Compartido")]
        public bool EsCompartido { get; set; }

        [DisplayName("Recibo y embarque")]
        public bool EsReciboYEmbarque { get; set; }

        [DisplayName("Doble")]
        public bool EsDoble { get; set; }

        [DisplayName("Sencillo")]
        public bool EsSencillo { get; set; }

        [DisplayName("Paso")]
        public bool EsTienePaso { get; set; }

        [DisplayName("Cortina")]
        public bool EsTieneCortina { get; set; }



    }



}
