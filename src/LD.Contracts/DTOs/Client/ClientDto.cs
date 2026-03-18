using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Client
{
    public class ClientDto
    {
        [DisplayName("Activo")]
        public bool Activo { get; set; }

        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("Nombre Comercial")]
        public string NombreComercial { get; set; } = string.Empty;

        [DisplayName("Domicilio comercial")]
        public string DomicilioComercial { get; set; } = string.Empty;

        [DisplayName("Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [DisplayName("Ciudad")]
        public string Ciudad { get; set; } = string.Empty;

        [DisplayName("Código postal")]
        public string CodigoPostal { get; set; } = string.Empty;

        [DisplayName("Razón social")]
        public string RazonSocial { get; set; } = string.Empty;

        [DisplayName("RFC")]
        public string Rfc { get; set; } = string.Empty;

        [DisplayName("Domicilio fiscal")]
        public string DomicilioFiscal { get; set; } = string.Empty;

        [DisplayName("Colonia fiscal")]
        public string ColoniaFiscal { get; set; } = string.Empty;

        [DisplayName("Ciudad fiscal")]
        public string CiudadFiscal { get; set; } = string.Empty;

        [DisplayName("Codigo postal fiscal")]
        public string CodigoPostalFiscal { get; set; } = string.Empty;

        [DisplayName("Email fiscal")]
        public string EmailFiscal { get; set; } = string.Empty;

        [DisplayName("Teléfono fiscal")]
        public string TelefonoFiscal { get; set; } = string.Empty;

        [DisplayName("Proveedor VMI")]
        public bool IsProvider { get; set; }


    }

}
