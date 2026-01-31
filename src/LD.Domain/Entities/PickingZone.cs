using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class PickingZone : AuditableEntity
    {
        [Key]
        public int PickingZoneId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int ProyectId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string PartNumber { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        public int Minimun { get; set; }

        public bool Particioned { get; set; }


        // Navegación
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        //public Proyecto Proyecto { get; set; }
        //public Producto Producto { get; set; }
        [ForeignKey("LocationId")]
        public Location Location { get; set; }
    }

}
