using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.Model
{
    public class Turno
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<Factura> Facturas { get; set; } = new List<Factura>();
        public virtual List<DetalleTurno> DetalleTurnos { get; set; } = new List<DetalleTurno>();
        public virtual List<ContratoTractor> ContratosTractor { get; set; } = new List<ContratoTractor>();
        public string Estado { get; set; } = null!;
        public bool Abierto { get; set; }
        public decimal TotalInicial { get; set; }
        public decimal Ventas { get; set; }
        public decimal Gastos { get; set; }
        public decimal OtrosIngresos { get; set; }
        public decimal Efectivo { get; set; }
        public decimal Tarjetas { get; set; }
        public decimal Transferencias { get; set; }
        public decimal Cheques { get; set; }
        public decimal Diferencia { get; set; }
        public decimal DifernciaCalculada=> Efectivo - TotalFinal;
        public decimal EfectivoContado { get; set; }
        public decimal TotalFinal { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public string? Color { get; set; }
        public virtual List<DesgloseBilletes> DesgloseBilletes { get; set; } = new List<DesgloseBilletes>();
    }
}
