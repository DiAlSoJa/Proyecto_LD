using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Project : AuditableEntity
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }


        // Flags operativos
        public bool Backorder { get; set; }
        public bool Distribution { get; set; }
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
        public Client Client { get; set; }
        //public Almacen Almacen { get; set; }

        //public ProyectoFlujoConfig FlujoConfig { get; set; }
        //public ProyectoPrecioConfig PrecioConfig { get; set; }
        //public ProyectoNotificacionConfig NotificacionConfig { get; set; }
    }

}
