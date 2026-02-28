using System;
using System.Collections.Generic;
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

        public string? ProjectName { get; set; } = string.Empty;


        public bool ScanDub { get; set; } = false;

        public bool ScanPartNumber { get; set; } = false;

        public bool ScanQuantity { get; set; } = false;

        public bool RequireLot { get; set; } = false;

        public bool RequireExpirationDate { get; set; } = false;
    }

}
