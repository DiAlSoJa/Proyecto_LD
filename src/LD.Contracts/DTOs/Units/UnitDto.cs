
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Units
{
    public class UnitDto
    {
        [DisplayName("Unidad")]
        public string Unidad { get; set; } = string.Empty;
        [DisplayName("Descripción")]
        public string Descripcion { get; set; } = string.Empty;
    }


}

