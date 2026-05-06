using System;
using System.Collections.Generic;
using System.Text;

namespace SGTA.Model
{
    public class Citas
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public virtual Vehiculo? Vehiculo { get; set; } 
    }
}
