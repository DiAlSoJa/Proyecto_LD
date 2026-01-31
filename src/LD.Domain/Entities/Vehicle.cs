using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Vehicle : AuditableEntity
    {
        [Key]
        public int VehicleId { get; set; }

        [Required]
        [MaxLength(20)]
        public string VehicleNumber { get; set; }   // No. Vehículo

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }            // FORD DIESEL 96

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }              // CAJA SECA, RABON, CAMIONETA

        [Required]
        public decimal Capacity { get; set; }        // 5.00

        [MaxLength(50)]
        public string Placas { get; set; }            // JP26370

        // Dimensiones (metros)
        public decimal Largo { get; set; }            // 4.20
        public decimal Wight { get; set; }            // 2.30
        public decimal Height { get; set; }              // 1.90
    }
}
