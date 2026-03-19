using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SFCH.Model
{
   public class Cuenta
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; } = null!;
        public string NombreCuenta { get; set; } = null!;
        public DateTime FechaApertura { get; set; } = DateTime.Now;
        public decimal Saldo { get; set; }
        public decimal Interes { get; set; }
        public string Moneda { get; set; } = null!;
        public bool Activo { get; set; } = true;
        public virtual Persona Titular { get; set; } = null!;
        public virtual TipoCuenta TipoCuenta { get; set; } = null!;
        public virtual ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
        public override string ToString()
        {
            return $"{TipoCuenta} {NumeroCuenta}";
        }
    }
}
