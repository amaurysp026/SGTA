using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.Model
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string RutaImagen { get; set; } = "C:\\SFCH\\Data\\img\\logo.jpg";
        public virtual List<Producto> Productos { get; set; } = new List<Producto>();
        public int CantProducto => Productos.Count();
        public decimal ValorInventario => Productos.Sum(x => x.CantidadDisponible * x.Costo);
           
        public bool Activo { get; set; }
        public override string ToString()
        {
            return Nombre;
        }
    }
}
