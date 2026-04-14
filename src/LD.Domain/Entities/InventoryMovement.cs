using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using LD.Domain.Common;
using LD.Domain.Enums;


namespace LD.Domain.Entities
{
    public class InventoryMovement : AuditableEntity
    {
        [Key]
        [Required]
        public int MovementId { get; set; }

        [Required]        
        public int? ProductId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PartNumber { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public String UserId { get; set; }

        [MaxLength(50)]
        public string? LotNumber { get; set; }

        [MaxLength(100)]
        public string? Reference { get; set; }

        [MaxLength(50)]
        public string? PurchaseOrder { get; set; }

        [MaxLength(50)]
        public string? CustomsDeclarationNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public DocumentType_e DocumentType { get; set; } // ajuste ,transferencia, entrada, salida, conteo, etc
        public MovementType_e MovementType { get; set; } // entrada, salida

        [MaxLength(50)]
        public string DocumentId { get; set; }

        [MaxLength(30)]
        public string? StatusId { get; set; }
    

        public int? LocationId{ get; set; }       

        public decimal? Qty { get; set; }

        public int? StandardId { get; set; }

        
        public Product? Product { get; set; }
        public Location? Location { get; set; }
        public Client? Client { get; set; }
        public Project? Project { get; set; }
    }
}
