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
        public int ProjectId { get; set; }
        public string Proyecto { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string Almacen { get; set; } = string.Empty;

        public bool EscaneoDub { get; set; }
        public bool EscaneoNumeroParte { get; set; }
        public bool EscaneoCantidad { get; set; }
        public bool RequiereLote { get; set; }
        public bool RequiereFechaCaducidad { get; set; }

 
        public bool NotificacionInterna { get; set; }
        public bool NotificacionRecibo { get; set; }
        public bool NotificacionEmbarque { get; set; }
        public bool Backorder { get; set; }
        public bool Distribucion { get; set; }
        public bool SD { get; set; }
        public bool Fiscal { get; set; }
        public bool Etiqueta { get; set; }

        public int AP { get; set; } // parece numérico (0,1,...)

        public bool Valid { get; set; }
    }

}
