using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class ProjectRequest
    {
        public int? ProjectId { get; set; }

        public int? ClientId { get; set; }
        public int? WarehouseId { get; set; }
        public int? StorageTypeId { get; set; }
        public string? ProjectName { get; set; } = string.Empty;
        public bool AutoPicking { get; set; }

        public bool AllowsBackorder { get; set; }

        public bool IsDistributionArea { get; set; }

        public bool IsFiscalWarehouse { get; set; }

        public bool AllowsOversizedItems { get; set; }

        public bool RequiresLabels { get; set; }
        public string Entrada { get; set; } = string.Empty;
        public string StorageArea { get; set; } = string.Empty;
        public string ReworkArea { get; set; } = string.Empty;
        public string Salida { get; set; } = string.Empty;

        public bool ReceiptNotificationEnabled { get; set; }
        public string? ReceiptNotificationMethod { get; set; }

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

        public bool IsActive { get; set; }

        public List<ScanConfigurationRequest> ScanConfigurations { get; set; } = [];
    }

}
