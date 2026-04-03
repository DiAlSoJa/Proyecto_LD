using System;

namespace LD.Contracts.Requests
{
    public class AsnRequest
    {
        public int AsnId { get; set; }
        public string AsnCode { get; set; }
        public string? PreAsnCode { get; set; }

        public int ClientId { get; set; }
        public int ProjectId { get; set; }

        public string? InvoiceNumber { get; set; }
        public string? GuideNumber { get; set; }

        public DateTime? Eta { get; set; }
        public int? PackagesQty { get; set; }

        public bool IsReturn { get; set; }
        public bool IsCustomerMovementRequired { get; set; }

        public string? TransportLine { get; set; }
        public string? VehicleType { get; set; }
        public string? DriverName { get; set; }
        public string? VehiclePlate { get; set; }
        public string? SealNumber { get; set; }

        public string? Status { get; set; }
    }
}
