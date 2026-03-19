using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.Model
{
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Identificacion { get; set; } = null!;
        public string TipoIdentificacion { get; set; } = null!;
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Celular { get; set; }
        public string? Email { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Edad
        {
            get
            {
                int Ed = DateTime.Now.Year - FechaNacimiento.Year;
                if (DateTime.Now.Month < FechaNacimiento.Month)
                {
                    Ed -= 1;
                }
                return Ed;
            }
        }
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
        public int Antiguedad
        {
            get
            {
                int Ed = DateTime.Now.Year - FechaIngreso.Year;
                if (DateTime.Now.Month < FechaNacimiento.Month)
                {
                    Ed -= 1;
                }
                return Ed;
            }
        }
        public string? CodSocioAsogafar { get; set; } = string.Empty;
        public bool DescuentoAsogafar { get; set; } = false;
        public decimal MontoDescuento { get; set; } = 0.0m;
        public string? Observacion { get; set; }
        public virtual TipoEntidad TipoEntidad { get; set; } = null!;
        public virtual ICollection<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
        public virtual ICollection<CxC> CxCs { get; set; } = new List<CxC>();
        public decimal Deuda => CxCs.Where(x => x.Pagado == false && x.Anulado == false).Sum(x => x.MontoPendiente);
        public override string ToString()
        {
            return $"{Nombre} {Apellido}";
        }

    }
}
