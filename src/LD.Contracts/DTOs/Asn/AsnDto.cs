using System;
using System.ComponentModel;

namespace LD.Contracts.ASN
{
    public class AsnDto
    {
        [DisplayName("Id")]
        public int AsnId { get; set; }

        [DisplayName("Creado")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("ASN")]
        public string AsnCode { get; set; } = string.Empty;

       

        [DisplayName("Cliente")]
        public string Client { get; set; } = string.Empty;

      
        [DisplayName("Proyecto")]
        public string Project { get; set; } = string.Empty;

    

        [DisplayName("Factura")]
        public string InvoiceNumber { get; set; } = string.Empty;

        [DisplayName("Guía")]
        public string GuideNumber { get; set; } = string.Empty;

        [DisplayName("ETA")]
        public DateTime? Eta { get; set; }

        [DisplayName("Bultos")]
        public int? PackagesQty { get; set; }

        [DisplayName("Devolución")]
        public bool IsReturn { get; set; }

        [DisplayName("Mov. requerido cliente")]
        public bool IsCustomerMovementRequired { get; set; }

        [DisplayName("Línea transporte")]
        public string TransportLine { get; set; } = string.Empty;

        [DisplayName("Tipo vehículo")]
        public string VehicleType { get; set; } = string.Empty;

        [DisplayName("Chofer")]
        public string DriverName { get; set; } = string.Empty;

        [DisplayName("Placas")]
        public string VehiclePlate { get; set; } = string.Empty;

        [DisplayName("Sello")]
        public string SealNumber { get; set; } = string.Empty;

        [DisplayName("Status")]
        public string Status { get; set; } = string.Empty;
    }
}
