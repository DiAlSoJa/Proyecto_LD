using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class CategoryRequest
    {
        public string CategoryIdS { get; set; }   


        public string? Description { get; set; }
        public int? Frecuency { get; set; }        

        public int ClientId { get; set; }
        public int ProjectId { get; set; }

    }


}

