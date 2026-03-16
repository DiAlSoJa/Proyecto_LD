using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class StatusRequest
    {


        public int StatusId { get; set; }
   
        public string Clave { get; set; }   // A, B

        
        public string Description { get; set; } // CADUCADO , DETENIDO


        public bool IsAvailable { get; set; } // Disponible o No Disponible



    }


}

