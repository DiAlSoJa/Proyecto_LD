using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class Equipment : AuditableEntity
    {
        [Key]
        public int EquipmentId { get; set; }

        [Required]
        public string EquipmentName { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        public string? Brand { get; set; }

        public decimal? Hourmeter { get; set; }
        public bool IsOperative { get; set; }

        public int EquipmentTypeId { get; set; }
        public int WarehouseId { get; set; }    
        public int EquipmentSupplierId { get; set; }
        public string Turn1 { get; set; } = string.Empty;
        public string Turn2 { get; set; } = string.Empty;
        public string Turn3 { get; set; } = string.Empty;
        public string ImagePathLeft { get; set; } = string.Empty;
        public string ImagePathRight { get; set; } = string.Empty;

        public EquipmentType? EquipmentType { get; set; }
        public Warehouse? Warehouse { get; set; }
        public EquipmentSupplier? EquipmentSupplier { get; set; }

    }
}
