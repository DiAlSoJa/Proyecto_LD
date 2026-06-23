using System;
using System.ComponentModel;

namespace LD.Contracts.Kitting
{
    public class KittingDto
    {
        [DisplayName("Id")]
        public int KittingId { get; set; }

        [DisplayName("Kitting")]
        public string KittingCode { get; set; } = string.Empty;

        [DisplayName("Cliente")]
        public string Client { get; set; } = string.Empty;

        [DisplayName("Proyecto")]
        public string Project { get; set; } = string.Empty;

        [DisplayName("Almacén")]
        public string Warehouse { get; set; } = string.Empty;

        [DisplayName("Factura")]
        public string InvoiceNumber { get; set; } = string.Empty;

        [DisplayName("Guia")]
        public string GuideNumber { get; set; } = string.Empty;

        [DisplayName("ETA")]
        public DateTime? Eta { get; set; }

        [DisplayName("Bultos")]
        public int? PackagesQty { get; set; }

        [DisplayName("Devolucion")]
        public bool IsReturn { get; set; }

        [DisplayName("Mov. requerido cliente")]
        public bool IsCustomerMovementRequired { get; set; }

        [DisplayName("Linea transporte")]
        public string TransportLine { get; set; } = string.Empty;

        [DisplayName("Tipo vehiculo")]
        public string VehicleType { get; set; } = string.Empty;

        [DisplayName("Chofer")]
        public string DriverName { get; set; } = string.Empty;

        [DisplayName("Placas")]
        public string VehiclePlate { get; set; } = string.Empty;

        [DisplayName("Sello")]
        public string SealNumber { get; set; } = string.Empty;

        [DisplayName("Contacto")]
        public string Contacto { get; set; } = string.Empty;

        [DisplayName("Direccion")]
        public string Direccion { get; set; } = string.Empty;

        [DisplayName("Colonia")]
        public string Colonia { get; set; } = string.Empty;

        [DisplayName("Ciudad")]
        public string Ciudad { get; set; } = string.Empty;

        [DisplayName("Telefono")]
        public string Telefono { get; set; } = string.Empty;

        [DisplayName("Codigo Postal")]
        public string CodigoPostal { get; set; } = string.Empty;

        [DisplayName("Tipo Entrega")]
        public string TipoEntrega { get; set; } = string.Empty;

        [DisplayName("Fecha Programada")]
        public DateTime? FechaProgramada { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; } = string.Empty;

        public string? Photo1Path { get; set; }
        public string? Photo2Path { get; set; }
        public string? Photo3Path { get; set; }
        public string? Photo4Path { get; set; }
    }
}
