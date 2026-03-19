using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class Lote
    {
        public int Id { get; set; }
        public string CodigoLote { get; set; }=string.Empty;
        public DateTime FechaVencimiento { get; set; }
        public DateTime? FechaFabricacion { get; set; }
        public virtual Producto Producto { get; set; }=null!;
        public decimal Cantidad { get; set; }
        public DateTime FechaEntrada { get; set; } = DateTime.Now;
       public Usuario Usuario { get; set; }=null!;
        public string? Color { get; set; }

        public override string ToString()
        {
            return FechaVencimiento.ToString("d");
        }

    }
}
