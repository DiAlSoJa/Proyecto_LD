
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Vehicle
{
    public class VehicleDto
    {
        public int VehicleId { get; set; }
        public string NumeroVehiculo { get; set; }   // No. Vehículo        
        public string Nombre { get; set; }            // FORD DIESEL 96        
        public string Tipo { get; set; }              // CAJA SECA, RABON, CAMIONETA
        public decimal Capacidad { get; set; }        // 5.00        
        public string Placas { get; set; }            // JP26370
        // Dimensiones (metros)
        public decimal Largo { get; set; }            // 4.20
        public decimal Ancho { get; set; }            // 2.30
        public decimal Alto { get; set; }              // 1.90
    }


}


