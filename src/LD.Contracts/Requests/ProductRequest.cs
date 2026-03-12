using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class ProductRequest
    {
        public int? ItemId { get; set; }

        public int? ProjectId { get; set; }

        public string? PartNumber { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;


        public int? MinUnitId { get; set; }

        public int? MediumUnitId { get; set; }

        public int? MaxUnitId { get; set; }

        public bool? RequestLotNumber { get; set; } = false;

        public bool? RequestExpirationDate { get; set; } = false;
    }

}
