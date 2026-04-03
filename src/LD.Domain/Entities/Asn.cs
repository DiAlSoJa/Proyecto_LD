using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities
{
    public class Asn : AuditableEntity
    {
        [Key]
        [Required]
        public int AsnId { get; set; }

        [MaxLength(30)]
        public string? PreAsnCode { get; set; }

        [MaxLength(30)]
        public string? AsnCode { get; set; }

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

        [MaxLength(30)]
        public string? Status { get; set; }

        public Client? Client { get; set; }
        public Project? Project { get; set; }

        public ICollection<AsnDetail> AsnDetails { get; set; } = new List<AsnDetail>();
    }
}