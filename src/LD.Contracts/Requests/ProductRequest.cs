using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class ProductRequest
    {
        public int ProductId { get; set; }

        public int? ProjectId { get; set; }
        public int? ClientId { get; set; }
        public int? CategoryId { get; set; }
        public string? DimensionerId { get; set; }

        public string? PartNumber { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool? IsTemperatureControlled { get; set; } = false;
        public bool? IsVMI { get; set; } = false;
        public bool? IsBOM { get; set; } = false;
        public bool? RequestLotNumber { get; set; } = false;
        public bool? RequestExpirationDate { get; set; } = false;
        public bool? RequestDeclarationNumber { get; set; } = false; // número de pedimento
        public bool? RequestExchangeRate { get; set; } = false;
        public bool? RequestPurchaseOrder { get; set; } = false;
        public bool? RequestReference { get; set; } = false;
        public int? StorageTypeId { get; set; }
        public string? MinUnitId { get; set; }
        public string? MediumUnitId { get; set; }
        public string? MaxUnitId { get; set; }
        public string? StandardPackage { get; set; }
        public decimal? MediumUnitValue { get; set; }
        public decimal? MaxUnitValue { get; set; }
        public decimal? StandardPackageValue { get; set; }
        public decimal? Costs { get; set; }
        public decimal? WarehouseFactor { get; set; }
        public decimal? ProductionFactor { get; set; }
        public string? ProductionUnitId { get; set; }
        public string? ProductionStatusId { get; set; }
        public bool? RequestNotificationMax { get; set; }
        public bool? RequestNotificationMin { get; set; }
        public decimal? Maximums { get; set; }
        public decimal? Minimus { get; set; }
        public int? DeliveryTime { get; set; }
        public decimal? Reorder { get; set; }
        public bool? RequestNotificationEmail { get; set; }
        public bool? RequestNotificationFiles { get; set; }
        public string? DistributionList { get; set; }
        public string? NotificationRoute { get; set; }
        public string? AlternateEmail { get; set; }
        public int? FamilyId { get; set; }

        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public decimal? Weight { get; set; }

     
    }

}
