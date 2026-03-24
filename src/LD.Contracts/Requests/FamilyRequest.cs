using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class FamilyRequest
    {
        public int FamilyId { get; set; }
        public string FamilyName { get; set; }

        public int WarehouseId { get; set; }
        public int ProjectId { get; set; }


    }


}

