using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Location
{
    public class LocationDto
    {
        public int LocationId { get; set; }
        public bool Activo { get; set; }

        public int AlmacenId { get; set; }
        public string Almacen { get; set; } = string.Empty;

        //public string Pasillo { get; set; } = string.Empty;

        //public string Nivel { get; set; } = string.Empty;

        public string Ubicacion { get; set; } = string.Empty;

        public string Dimension { get; set; } = string.Empty;

        public bool Fiscal { get; set; }


        public bool ControlTemperatura { get; set; }
        public bool Rack { get; set; }
        public bool General { get; set; }
        public bool Cuarentena { get; set; }
        public bool Embarque { get; set; }
        public bool Compartido { get; set; }
        public bool ReciboYEmbarque { get; set; }
        public bool Doble { get; set; }
        public bool Sencillo { get; set; }
        public bool TienePaso { get; set; }
        public bool TieneCortina { get; set; }



    }



}
