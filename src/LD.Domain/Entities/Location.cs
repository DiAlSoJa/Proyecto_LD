using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Location : AuditableEntity
    {
        [Key]
        public int LocationId { get; set; }

        public int WarehouseId { get; set; }

        public string WarehouseCode { get; set; } = string.Empty;

        public string Rack { get; set; } = string.Empty;

        public string? Aisle { get; set; }

        public string? Level { get; set; }

        public string? LocationCode { get; set; }

        public string? Dimension { get; set; }

        public bool IsGeneral { get; set; }

        public bool IsReceipt { get; set; }

        public bool IsQuarantine { get; set; }

        public bool IsShipping { get; set; }

        public Warehouse? Warehouse { get; set; }

    }


}
