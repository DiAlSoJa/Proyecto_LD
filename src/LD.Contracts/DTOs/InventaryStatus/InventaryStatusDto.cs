using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.InventaryStatus
{
    public class InventaryStatusDto
    {
        public int StatusId { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Disponible { get; set; } = false;
    }


}

