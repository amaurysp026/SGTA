using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class DetalleLiquidacionLeche
    {
        public int Id { get; set; }
        public virtual LiquidacionLeche LiquidacionLeche { get; set; }
        public string CodSocio { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public decimal Litros { get; set; }
        public decimal Valor { get;set;  }
        public decimal TotalDescuento { get; set; }
        public decimal NetoCobrar { get; set; }
    }
}
