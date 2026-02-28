using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class LocationRequest
    {
        public int? LocationId { get; set; }

        public int WarehouseId { get; set; }
        // Dimensions (cm)
        public decimal? Height { get; set; }

        public decimal? Width { get; set; }

        public decimal? Depth { get; set; }

        public string Rack { get; set; } = string.Empty;

        public string? Aisle { get; set; }

        public string? Level { get; set; }

        public string? LocationCode { get; set; }

        public string? Dimension { get; set; }

        public bool IsGeneral { get; set; }

        public bool IsReceipt { get; set; }

        public bool IsQuarantine { get; set; }

        public bool IsShipping { get; set; }

        public bool IsActive { get; set; }
    }

}
