using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SFCH.Model
{
    public class DetalleTurno
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }= DateTime.Now;
        public virtual Turno Turno { get; set; }
        public string Descripcion { get; set; }

        public decimal Ingreso { get; set; }
        public decimal Egreso { get; set; }

    }
}
