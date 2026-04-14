using System;
using System.Collections.Generic;
using System.Text;

namespace SGTA.Model
{
    public class MantVehiculo
    {
        public int Id { get; set; } 
        public string Descripcion { get; set; } = "";
        public decimal Odometro { get; set; } 
        public decimal ProximoOdometro { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaProximo { get; set;  }
        public Vehiculo Vehiculo { get; set; } = null!;
    }
}
