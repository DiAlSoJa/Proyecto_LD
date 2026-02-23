using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Project
{
    public class ProjectDto
    {
        public bool Activo { get; set; }

        public int ClienteId { get; set; }
        public string Cliente { get; set; } = string.Empty;

        public int ProjectId { get; set; }
        public string Proyecto { get; set; } = string.Empty;

        public int AlmacenId { get; set; }
        public string Almacen { get; set; } = string.Empty;

        public bool EscaneoDub { get; set; }

        public bool EscaneoNumeroParte { get; set; }

        public bool EscaneoCantidad { get; set; }

        public bool RequiereLote { get; set; }

        public bool RequiereFechaCaducidad { get; set; }
    }


}
