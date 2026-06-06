using System;

namespace LD.Contracts.Requests
{
    public class KittingRequest
    {
        public int KittingId { get; set; }
        public string? PreKittingCode { get; set; }
        public string? KittingCode { get; set; }

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

        public string? Contacto { get; set; }
        public string? Direccion { get; set; }
        public string? Colonia { get; set; }
        public string? Ciudad { get; set; }
        public string? Telefono { get; set; }
        public string? CodigoPostal { get; set; }
        public string? TipoEntrega { get; set; }
        public DateTime? FechaProgramada { get; set; }

        public string? Status { get; set; }
    }
}
