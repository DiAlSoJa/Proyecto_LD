using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class LocationRequest
    {
        public int? LocationId { get; set; }

        public int WarehouseId { get; set; }
        public string? LocationName { get; set; }
        public bool IsFiscal { get; set; }
        public bool HasControlledTemperature { get; set; }
        // Dimensions (cm)
        public decimal? Height { get; set; }

        public decimal? Width { get; set; }

        public decimal? Depth { get; set; }

        // ===== Tipo =====

        public bool IsRack { get; set; }

        public bool IsCompartidoType { get; set; }

        // ===== sub Tipo =====
        public bool IsGeneral { get; set; }

        public bool IsCuarentena { get; set; }

        public bool IsEmbarque { get; set; }

        public bool IsCompartido { get; set; }

        public bool IsReciboYEmbarque { get; set; }

        // ===== Tamaño =====

        public bool IsDoble { get; set; }

        public bool IsSencillo { get; set; }

        // ===== Extras =====

        public bool HasPaso { get; set; }

        public bool HasCortina { get; set; }


        public bool IsActive { get; set; }

        // Para creación masiva
        public string Rack {  get; set; }
        public int FromW { get; set; }
        public int ToW { get; set; }
        public int Leves { get; set; }
    }

}
