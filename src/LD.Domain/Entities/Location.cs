using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Location : AuditableEntity
    {
        [Key]
        public int LocationId { get; set; }

        public int WarehouseId { get; set; }
        public string LocationName { get; set; }


        public bool IsFiscal { get; set; }

        public bool HasControlledTemperature { get; set; }

        public decimal? Height { get; set; }

        public decimal? Width { get; set; }

        public decimal? Depth { get; set; }



        // ===== Tipo =====

        public bool IsRack { get; set; }
        public bool IsCompartidoType { get; set; }


        // ===== SubTipo =====
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

        public Warehouse Warehouse { get; set; }

        // RAck nivel y posición
        [MaxLength(10)]
        public string? Rack { get; set; }
        
        [MaxLength(10)]
        public string? Level { get; set; }
        [MaxLength(10)]
        public string? Position { get; set; }

    }


}
