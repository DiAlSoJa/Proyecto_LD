using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Warehouse
{
    public class WarehouseDto
    {
        public bool Activo { get; set; }

        public int Id { get; set; }

        public string NombreAlmacen { get; set; } = string.Empty;

        public string Domicilio { get; set; } = string.Empty;

        public string Colonia { get; set; } = string.Empty;

        public string Ciudad { get; set; } = string.Empty;

        public string CodigoPostal { get; set; } = string.Empty;
    }


}
