using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LD.Contracts.EquipmentType
{
    public class EquipmentTypeDto
    {
        [DisplayName("Id")]
        public int EquipmentTypeId { get; set; }

        [DisplayName("Equipo")]
        public string EquipmentName { get; set; }

        [DisplayName("Usa batería")]
        public bool IsBattery { get; set; }

        [DisplayName("Imagen izquierda")]
        public string ImagePathLeft { get; set; } = string.Empty;

        [DisplayName("Imagen derecha")]
        public string ImagePathRight { get; set; } = string.Empty;
    }
}
