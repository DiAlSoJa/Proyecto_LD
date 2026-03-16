using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class InventaryStatusRequest
    {
        public int InventoryStatusId { get; set; }
        public string Clave { get; set; }   // A, C, D, E...
        public string FullName { get; set; }  // DISPONIBLE, CUARENTENA, etc.
        public bool IsAvailable { get; set; } // Impacta stock disponible
    }


}

