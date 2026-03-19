using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class DetalleCompra
    {
        public int Id { get; set; }
        public virtual Compra Compra { get; set; } = null!;
        public virtual Producto Producto { get; set; } = null!;
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get;set; }  
        public decimal PrecioUSD {
            get { return field; }
            set
            {
               
                    field = value;


                PrecioUnitario = PrecioUSD * Tasa;
            }
        }
        public decimal Descuento { get; set; }
        public decimal Tasa { get; set; }
        public decimal Impuesto
        {
            get;set;
        }
        public decimal SubttalUSD
        {
            get { return PrecioUSD * Cantidad + Impuesto - Descuento; }
        }

        public decimal Subtotal
        {
            get { return PrecioUnitario * Cantidad + Impuesto - Descuento; }
        }
    }
}
