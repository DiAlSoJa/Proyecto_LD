using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class Status : AuditableEntity
    {
        [Key]
        public int StatusId { get; set; }

        [Required]
        [MaxLength(10)]
        public string Clave { get; set; }   // A, B

        [Required]
        [MaxLength(100)]
        public string Description { get; set; } // CADUCADO , DETENIDO


        public bool IsAvailable { get; set; } = false; // Disponible o No Disponible


    }
}
