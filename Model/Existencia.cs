using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class Existencia
    {
        public int Id { get; set; }
        public virtual Producto Producto { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }=DateTime.Now;
        public decimal CostoUnitario { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string? Documento { get; set; }= null;
        public decimal Entrada { get; set; }
        public decimal Salida { get; set; }
        public decimal CantidadTotal { get; set; }
        public string Descripcion { get; set; } = null!;
        public Lote? Lote { get; set; }
        public string NombreUsuario { get; set; }=string.Empty;
        public string? Color { get; set; }
        override public string ToString()
        {
            return $"{Producto.Nombre} - {CantidadTotal} unidades";
        }
    }
}
