using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class DetalleProducto
    {
        public int Id { get; set; }
        public virtual Producto Producto { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public decimal Cantidad { get; set; }
        public decimal Precio { get;set;  }
        public decimal Costo { get; set; }
        public decimal TotalPrecio { get; set; }
        public decimal TotalCosoto { get; set; }
    }
}
