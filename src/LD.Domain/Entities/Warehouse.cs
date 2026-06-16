using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class Warehouse : AuditableEntity
    {
        [Key]
        public int WarehouseId { get; set; } 

        [Required]
        [MaxLength(150)]
        public string WarehouseName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string Neighborhood { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(10)]
        public string ZipCode { get; set; }
        public decimal Capacity { get; set; }
        public bool IsProduction { get; set; } = false;

        public ICollection<UserWarehouse> UserWarehouses { get; set; } = new List<UserWarehouse>();
        public ICollection<OperationalTask> OperationalTasks { get; set; } = new List<OperationalTask>();
        public ICollection<WarehouseTask> WarehouseTasks { get; set; } = new List<WarehouseTask>();
    }
}
