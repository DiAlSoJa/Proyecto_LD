using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Driver : AuditableEntity
    {
        [Key]
        public int DriverId { get; set; }

        [Required]
        [MaxLength(20)]
        public string DriverNumber { get; set; }   // No. Chofer

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string Licence { get; set; }

        [MaxLength(20)]
        public string IMSS { get; set; }

    }
}
