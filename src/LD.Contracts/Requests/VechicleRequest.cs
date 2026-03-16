using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Requests
{
    public class VechicleRequest
    {

        public int VehicleId { get; set; }        
        public string VehicleNumber { get; set; }   // No. Vehículo        
        public string Name { get; set; }            // FORD DIESEL 96        
        public string Type { get; set; }              // CAJA SECA, RABON, CAMIONETA
        public decimal Capacity { get; set; }        // 5.00        
        public string Plates { get; set; }            // JP26370

        // Dimensiones (metros)
        public decimal Long { get; set; }            // 4.20
        public decimal Wight { get; set; }            // 2.30
        public decimal Height { get; set; }              // 1.90



    }


}

