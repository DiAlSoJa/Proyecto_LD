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
        public int StorageTypeId { get; set; }

        [Required]
        [MaxLength(150)]
        public string? ProjectName { get; set; }

        public bool AutoPicking { get; set; }

        public bool AllowsBackorder { get; set; }

        public bool IsDistributionArea { get; set; }

        public bool IsFiscalWarehouse { get; set; }

        public bool AllowsOversizedItems { get; set; }

        public bool RequiresLabels { get; set; }

        [MaxLength(50)]
        public string Entrada { get; set; } = string.Empty;
        [MaxLength(50)]
        public string StorageArea { get; set; } = string.Empty;
        [MaxLength(50)]
        public string ReworkArea { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Salida { get; set; } = string.Empty;


        public bool ReceiptNotificationEnabled { get; set; }
        public string? ReceiptNotificationMethod{ get; set; }

        public bool ShipmentNotificationEnabled { get; set; }
        public string? ShipmentNotificationMethod { get; set; }

        public bool InternalNotificationEnabled { get; set; }
        public string? InternalNotificationMethod { get; set; }

        public decimal? NormalHrs { get; set; }
        public decimal? UrgentHrs { get; set; }

        public string? AsnNumber { get; set; }
        public string? AsnPrefix { get; set; }

        // Kitting
        public string? KittingNumber { get; set; }
        public string? KittingPrefix { get; set; }

        // Delivery Order (DO)
        public string? DeliveryOrderNumber { get; set; }
        public string? DeliveryOrderPrefix { get; set; }

        public string? DoNumber { get; set; }
        public string? DoPrefix { get; set; }
        public bool ReciveRequired { get; set; }
        // Navegación
        public Client? Client { get; set; }
        public Warehouse? Warehouse { get; set; }
        public StorageType? StorageType{ get; set; }



    }

}
