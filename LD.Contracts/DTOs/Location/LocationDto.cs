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

        public string Rack { get; set; } = string.Empty;

        public string Pasillo { get; set; } = string.Empty;

        public string Nivel { get; set; } = string.Empty;

        public string Ubicacion { get; set; } = string.Empty;

        public string Dimension { get; set; } = string.Empty;

        public bool Usado { get; set; }

        public bool General { get; set; }

        public bool Recibo { get; set; }

        public bool Cuarentena { get; set; }

        public bool Embarque { get; set; }

        public bool EsRack { get; set; }
    }



}
