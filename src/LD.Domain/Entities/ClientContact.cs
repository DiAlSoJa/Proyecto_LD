using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class ClientContact : AuditableEntity
    {
        [Key]
        public int ContactId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; }

        [MaxLength(100)]
        public string Puesto { get; set; }

        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(20)]
        public string Fax { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }


        // Navegación
        public Client Client { get; set; }
    }
}
