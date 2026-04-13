using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Product
{
    public class ProductAutocompleteDto
    {
        [DisplayName("Id")]
        public int ItemId { get; set; }

        [DisplayName("No. parte")]
        public string NumeroParte { get; set; } = string.Empty;

        [DisplayName("Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        [DisplayName("Paquete estándar")]
        public decimal? StandardPackageValue { get; set; }

        [DisplayName("Unidad máxima")]
        public decimal? MaxUnitValue { get; set; }

    }


}
