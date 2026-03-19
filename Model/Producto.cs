using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SFCH.Model
{
    public class Producto
    {
        public int Id { get; set; }
        public string Referencia { get; set; }=string.Empty;
        public string? CodFabricante {  get; set; }
        public string Nombre { get; set; }=null!;
        public string Descripcion { get; set; }=string.Empty;
        public decimal Precio { get; set; }
        public decimal PrecioUSD { get; set; }
        public decimal Costo { get; set; }
        public string? Color { get; set; }
        public byte[]? Imagen { get; set; }
        public decimal CostoUSD { get; set; }
        public decimal ITBIS { get; set; }
        public decimal Descuento { get; set; }
        public decimal Utilidad {             get
            {
                if (Costo == 0) return 0;
                return Math.Round(Precio - Costo,2);
            }
        }
        public string RutaImagen { get; set; } = "C:\\SFCH\\Data\\img\\logo.jpg";
        public virtual Categoria Categoria { get; set; }=null!;
        public virtual UnidadMedida Unidad{ get; set; } = null!;
        public virtual UnidadMedida? UnidadCompra { get; set; } 
        public DateTime FechaFabricacion { get; set; } = DateTime.Now;
        public decimal CantidadMinima { get; set; } = 0;
        public bool VenderSinExistencia { get; set; } = false;
        public bool CambiarPrecio { get; set; } = false;
        public bool PermitirStockNegativo { get; set; } = false;
        public bool Vence { get; set; } = false;
        public bool AplicaITBIS { get; set; } = true;
        public bool EsServicio { get; set; } = false;
        public bool AplicaDescuento { get; set; } = false;
        public bool AplicaCredito { get; set; } = false;
        public bool Compuesto { get;set; } = false;
        public virtual List<DetalleProducto> Detalles { get; set; }=new List<DetalleProducto>();
        public virtual List<Existencia> Existencias { get; set; } = new List<Existencia>();
       public  List<Lote> lotes = new List<Lote>();
        public decimal CantidadDisponible
        {
            get
            {
                decimal totalEntradas = Existencias.Sum(e => e.Entrada);
                decimal totalSalidas = Existencias.Sum(e => e.Salida);
                return totalEntradas - totalSalidas;
            }
        }
        public decimal ValorInventario
        {
            get
            {
                decimal totalEntradas = Existencias.Sum(e => e.Entrada);
                decimal totalSalidas = Existencias.Sum(e => e.Salida);
                return (totalEntradas - totalSalidas) * Costo;
            }
        }
        public decimal SalidaEsteMes
        {
            get
            {
                decimal totalsemana = Existencias.Where(x => x.Salida > 0 && x.FechaRegistro.Month == DateTime.Now.Month).Sum(x => x.Salida);
                return totalsemana;
            }
        }
        public decimal EntradasEsteMes
        {
            get
            {
                decimal totalsemana = Existencias.Where(x => x.Entrada > 0 && x.FechaRegistro.Month == DateTime.Now.Month).Sum(x => x.Entrada);
                return totalsemana;
            }
        }
        public bool Activo { get; set; } = true;
        public override string ToString()
        {
            return Nombre;
        }
    }
}
