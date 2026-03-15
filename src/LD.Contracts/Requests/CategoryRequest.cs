using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class CategoryRequest
    {


        public int? CategoryId { get; set; }  
        public string? Clave { get; set; }   


        public string? Description { get; set; }
        public string? ClientId { get; set; }
        public int Frecuencia { get; set; }



    }


}

