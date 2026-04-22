using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class xd
    {
        public int Id { get; set; }
        public virtual x RecepcionLeche { get; set; } = null!;
        public virtual Entidad Proveedor { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public decimal Litros { get; set; }
        public decimal Grasa { get; set; }
        public string? Tanda
        {
            get; set;   
        } 
        public virtual f? Freezer { get; set; } = null;
        public decimal SolidosTotales { get; set; }
        public decimal PrecioPorLitro { get; set; }
        public decimal Monto { get; set; }
        public bool Nulo { get; set; } = false;
    }
}
