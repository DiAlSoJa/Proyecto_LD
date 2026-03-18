using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.InventaryStatus
{
    public class InventaryStatusDto
    {
        [DisplayName("Estatus")]
        public string StatusId { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; } = string.Empty;
        [DisplayName("Disponible")]
        public bool Disponible { get; set; } = false;
    }


}

