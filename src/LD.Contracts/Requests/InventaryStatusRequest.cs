using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class InventaryStatusRequest
    {
        public string InventoryStatusIdS { get; set; } = string.Empty;   // A, C, D, E...
        public string FullName { get; set; } = string.Empty;  // DISPONIBLE, CUARENTENA, etc.
        public int? ClientId { get; set; }
        public int? ProjectId { get; set; }
        public bool IsAvailable { get; set; } // Impacta stock disponible
    }
}

