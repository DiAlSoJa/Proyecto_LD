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
        public int WarehouseId { get; set; }

        [Required]
        [MaxLength(150)]
        public string ProjectName { get; set; }


        public bool ScanDub { get; set; } = false;

        public bool ScanPartNumber { get; set; } = false;

        public bool ScanQuantity { get; set; } = false;

        public bool RequireLot { get; set; } = false;

        public bool RequireExpirationDate { get; set; } = false;



        // Navegación
        public Client? Client { get; set; }
        public Warehouse? Warehouse { get; set; }

     
    }

}
