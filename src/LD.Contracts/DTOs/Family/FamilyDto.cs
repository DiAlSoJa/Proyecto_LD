
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LD.Contracts.DTOs.Family
{
    public class FamilyDto
    {
        [DisplayName("Id")]
        public int FamiliaId { get; set; }

        [DisplayName("Nombre de familía")]
        public string NombreFamilia { get; set; }

        [DisplayName("Almacén")]
        public string Warehouse { get; set; } = string.Empty;
        public string Proyecto { get; set; } = string.Empty;
    }


}


