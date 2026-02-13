using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class WarehouseRequest
    {
        public int? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? Neighborhood { get; set; }

        public string? City { get; set; }

        public string? ZipCode { get; set; }

        public bool IsActive { get; set; }
    }


}
