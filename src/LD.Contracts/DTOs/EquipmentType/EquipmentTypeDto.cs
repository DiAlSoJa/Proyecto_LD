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
    }
}
