using System.ComponentModel.DataAnnotations;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class CyclicInventoryDetail : AuditableEntity
    {
        [Key]
        public int CyclicInventoryDetailId { get; set; }

        public int CyclicInventoryId { get; set; }
        public int LocationId { get; set; }
        public int TakeNumber { get; set; } = 1;

        public bool Counted { get; set; }
        public decimal? TheoreticalQty { get; set; }
        public decimal? PhysicalQty { get; set; }
        public decimal? SameLocationQty { get; set; }
        public decimal? AnotherLocationQty { get; set; }

        [MaxLength(100)]
        public string? FirstCountResult { get; set; }

        [MaxLength(100)]
        public string? SecondCountResult { get; set; }

        [MaxLength(100)]
        public string? ThirdCountResult { get; set; }

        [MaxLength(100)]
        public string? FourthCountResult { get; set; }

        [MaxLength(100)]
        public string? FinalResult { get; set; }

        [MaxLength(100)]
        public string? PartNumber { get; set; }

        public bool Scanned { get; set; }

        public CyclicInventory? CyclicInventory { get; set; }
        public Location? Location { get; set; }
        public ICollection<CyclicInventoryAvailableInventory> AvailableInventories { get; set; } = new List<CyclicInventoryAvailableInventory>();
    }
}
