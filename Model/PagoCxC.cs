using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class PagoCxC
    {
        public int Id { get; set; }
        public virtual CxC CxC { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public decimal Pagado { get; set; }
        public string? FormaPago { get; set; }
        public string? Referencia { get; set; }
        public string? Banco { get; set; }
        public string? Observacion { get; set; }
        public virtual Usuario Usuario { get; set; }
        public bool Anulado { get; set; } = false;
    }
}
