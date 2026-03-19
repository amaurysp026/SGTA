using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class DetalleFrezzer
    {
        public int Id { get; set; }
        public virtual Freezer Frezzer { get; set; } = null!;
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public decimal Salida { get; set; }
        public decimal Entrada { get; set; }
        public decimal Balance { get; set; }
    }
}