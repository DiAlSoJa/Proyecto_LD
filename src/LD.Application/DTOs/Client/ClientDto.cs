using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.DTOs.Client
{
    public class ClientDto
    {
        public bool Activo { get; set; }

        public int Id { get; set; }

        public string NombreComercial { get; set; } = string.Empty;

        public string RazonSocial { get; set; } = string.Empty;

        public string Rfc { get; set; } = string.Empty;

        public string DomicilioComercial { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Ciudad { get; set; } = string.Empty;

        public string CodigoPostal { get; set; } = string.Empty;
    }

}
