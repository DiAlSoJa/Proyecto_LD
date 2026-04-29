using System;
using System.Collections.Generic;
using System.ComponentModel;
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

     
 
        public bool NotificacionInterna { get; set; }
        public bool NotificacionRecibo { get; set; }
        public bool NotificacionEmbarque { get; set; }
        public bool Backorder { get; set; }
        public bool Distribucion { get; set; }
        public bool SD { get; set; }
        public bool Fiscal { get; set; }
        public bool Etiqueta { get; set; }
        [DisplayName("Escaneo Obligatorio")]
        public bool ScanRequired { get; set; }

       
    }

}
