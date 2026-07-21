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
        public int? LocationId { get; set; }
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

        // FK → Units.UnitIdS
        [MaxLength(20)]
        public string? Entrada { get; set; }
        [MaxLength(20)]
        public string? StorageArea { get; set; }
        [MaxLength(20)]
        public string? ReworkArea { get; set; }
        [MaxLength(20)]
        public string? Salida { get; set; }


        public bool ReceiptNotificationEnabled { get; set; }
        public string? ReceiptNotificationMethod{ get; set; }

        public bool ShipmentNotificationEnabled { get; set; }
        public string? ShipmentNotificationMethod { get; set; }

        public bool InternalNotificationEnabled { get; set; }
        public string? InternalNotificationMethod { get; set; }

        public decimal? NormalHrs { get; set; }
        public decimal? UrgentHrs { get; set; }

        public int? AsnNumber { get; set; }
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
        public bool ScanRequired { get; set; }
        public bool UniqueLot { get; set; }
        // Navegación
        public Client? Client { get; set; }
        public Warehouse? Warehouse { get; set; }
        public Location? Location { get; set; }
        public StorageType? StorageType { get; set; }

        public Units? EntradaUnit { get; set; }
        public Units? StorageAreaUnit { get; set; }
        public Units? ReworkAreaUnit { get; set; }
        public Units? SalidaUnit { get; set; }

        public ICollection<ScanConfiguration> ScanConfigurations { get; set; } = new List<ScanConfiguration>();

    }

}
