using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class UserWarehouse : AuditableEntity
    {
        public int UserWarehouseId { get; set; }
        public string? UserId { get; set; }

        public int? WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }

    }

}
