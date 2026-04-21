using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class EquipmentTypeRequest
    {
        public int EquipmentTypeId { get; set; }

        public string EquipmentName { get; set; }

        public bool IsBattery { get; set; }
    }
}
