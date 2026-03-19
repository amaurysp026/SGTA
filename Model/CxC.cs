using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.Model
{
    public class CxC
    {
        public int Id { get; set; }
        public virtual Persona? Cliente { get; set; }
        public virtual Entidad? Cliente2 { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public virtual Factura? Factura { get; set; }
        public string? Titular { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int DiasCredito { get; set; } = 30;
        public decimal MontoFactura { get; set; }
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public DateTime FechaVencimiento
        {
            get
            {
                return field;
            }

            set => field = FechaEmision.AddDays(DiasCredito);
        }
        public decimal MontoTotal { get; set; }
        public decimal MontoPendiente { get; set; }
        public bool Pagado { get; set; } = false;
        public bool Anulado { get; set; } = false;
        public virtual List<PagoCxC> Pagos { get; set; } = new List<PagoCxC>();
    }
}
