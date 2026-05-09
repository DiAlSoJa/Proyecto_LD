using System.ComponentModel.DataAnnotations;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class CyclicInventory : AuditableEntity
    {
        [Key]
        public int CyclicInventoryId { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [MaxLength(450)]
        public string AuditorUserId { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? AuditorName { get; set; }

        public int WarehouseId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Abierto";

        public DateTime? CompletedAt { get; set; }

        public Warehouse? Warehouse { get; set; }
        public ICollection<CyclicInventoryDetail> Details { get; set; } = new List<CyclicInventoryDetail>();
    }
}
