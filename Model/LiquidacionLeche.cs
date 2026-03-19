using SFCH.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class LiquidacionLeche
    {
        public int Id { get; set; }
        public string Numero { get; set; }=null!;
        public string? Observacion { get; set; }
        public virtual List<DetalleLiquidacionLeche>? Detalle { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }=DateTime.Now;
        public DateTime FechaEmision {  get; set; }=DateTime.Now;
        public Usuario Usuario { get; set; } = null!;
    }
}
