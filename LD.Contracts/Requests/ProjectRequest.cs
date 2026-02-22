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

        public int ClientId { get; set; }

        public string ProjectName { get; set; } = string.Empty;

        public int WarehouseId { get; set; }

        public bool ScanDub { get; set; }

        public bool ScanPartNumber { get; set; }

        public bool ScanQuantity { get; set; }

        public bool RequireLot { get; set; }

        public bool RequireExpirationDate { get; set; }
    }

}
