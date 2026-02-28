using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class WarehouseRequest
    {
        public int? WarehouseId { get; set; }

        public string? WarehouseName { get; set; } 

        public string? Address { get; set; }

        public string? Neighborhood { get; set; }

        public string? City { get; set; }

        public string? ZipCode { get; set; }
        public decimal? Capacity { get; set; }

        public bool IsProduction { get; set; } = false;

        public bool IsActive { get; set; } = true;
    }


}
