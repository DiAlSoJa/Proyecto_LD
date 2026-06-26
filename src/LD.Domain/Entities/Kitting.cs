using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities
{
    public class Kitting : AuditableEntity
    {
        [Key]
        [Required]
        public int KittingId { get; set; }

        [MaxLength(30)]
        public string? PreKittingCode { get; set; }

        [MaxLength(30)]
        public string? KittingCode { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [MaxLength(50)]
        public string? InvoiceNumber { get; set; }

        [MaxLength(50)]
        public string? GuideNumber { get; set; }

        public DateTime? Eta { get; set; }

        public int? PackagesQty { get; set; }

        public bool IsReturn { get; set; }

        public bool IsCustomerMovementRequired { get; set; }

        [MaxLength(150)]
        public string? TransportLine { get; set; }

        [MaxLength(100)]
        public string? VehicleType { get; set; }

        [MaxLength(150)]
        public string? DriverName { get; set; }

        [MaxLength(30)]
        public string? VehiclePlate { get; set; }

        [MaxLength(50)]
        public string? SealNumber { get; set; }

        [MaxLength(100)]
        public string? Cortina { get; set; }

        [MaxLength(100)]
        public string? Caja { get; set; }

        [MaxLength(150)]
        public string? Contacto { get; set; }

        [MaxLength(250)]
        public string? Direccion { get; set; }

        [MaxLength(100)]
        public string? Colonia { get; set; }

        [MaxLength(100)]
        public string? Ciudad { get; set; }

        [MaxLength(30)]
        public string? Telefono { get; set; }

        [MaxLength(15)]
        public string? CodigoPostal { get; set; }

        [MaxLength(20)]
        public string? TipoEntrega { get; set; }

        public DateTime? FechaProgramada { get; set; }

        [MaxLength(30)]
        public string? Status { get; set; }

        [MaxLength(500)]
        public string? Photo1Path { get; set; }

        [MaxLength(500)]
        public string? Photo2Path { get; set; }

        [MaxLength(500)]
        public string? Photo3Path { get; set; }

        [MaxLength(500)]
        public string? Photo4Path { get; set; }

        public Client? Client { get; set; }
        public Project? Project { get; set; }

        public ICollection<KittingValidationPhoto> ValidationPhotos { get; set; } = new List<KittingValidationPhoto>();
        public ICollection<KittingDetail> KittingDetails { get; set; } = new List<KittingDetail>();
    }
}
