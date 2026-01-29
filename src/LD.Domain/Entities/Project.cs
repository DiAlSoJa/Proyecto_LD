using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Proyect : AuditableEntity
    {
        [Key]
        public int ProyectoId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        [MaxLength(150)]
        public string NombreProyecto { get; set; }

        public bool Activo { get; set; }

        // Flags operativos
        public bool Backorder { get; set; }
        public bool Distribucion { get; set; }
        public bool AlmacenFiscal { get; set; }
        public bool Subdimension { get; set; }
        public bool Etiquetas { get; set; }

        // FIFO / Lote
        public bool UsaFIFO { get; set; }
        public bool UsaLote { get; set; }
        public bool NumeroDeLote { get; set; }
        public bool FechaCaducidad { get; set; }

        public int? AlmacenId { get; set; }

        // Navegación
        public Client Cliente { get; set; }
        //public Almacen Almacen { get; set; }

        //public ProyectoFlujoConfig FlujoConfig { get; set; }
        //public ProyectoPrecioConfig PrecioConfig { get; set; }
        //public ProyectoNotificacionConfig NotificacionConfig { get; set; }
    }

}
