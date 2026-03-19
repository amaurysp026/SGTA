using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;

namespace SFCH.Model
{
    public class Entidad
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string NombreCompleto { get; set; } = null!;
        public string? Cedula { get; set; }
        public string? Genero { get; set; }
        public string? Email { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Telefono { get; set; }
        public string? Celular { get; set; }
        public string? Direccion { get; set; }
        public DateTime FechaEntrada { get; set; }=DateTime.Now;
        public DateTime FechaNacimiento { get; set; }
        public string? CuentaBanco { get; set; }
        public string? Observaciones { get; set; }
        public bool Credito { get; set; }=true;
        public decimal LimiteCredito { get; set; }=100000;
        public int DiasCredito { get; set; }=15;
        public bool Socio { get; set; }=false;
        public TipoEntidadAso? TipoEntidadAso { get; set; }
        public virtual List<DetalleRecepcionLeche>  DetalleRecepcionLeches { get; set; } = new List<DetalleRecepcionLeche>();
        public virtual ICollection<CxC> CxCs { get; set; } = new List<CxC>();
        public decimal Deuda=> CxCs.Where(x => x.Pagado == false && x.Anulado == false).Sum(x => x.MontoPendiente);
        public bool Activo { get; set; }=true;
        public string? Color { get; set; }

        public override string ToString()
        {
            return NombreCompleto;
        }
    }
}
