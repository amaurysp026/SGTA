using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class Empleado
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public string? Direccion { get; set; } 
        public string? Telefono { get; set; }
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
        public string? Cargo { get; set; }
        public string Genero { get; set; }=null!;
        public bool Activo { get; set; }
        public virtual List<ContratoTractor>? ContratosTractor { get; set; }
        public decimal Salario { get; set; }
        public decimal Comision { get; set; }
        public DateTime FechadeIngreso { get; set; }
        public DateTime FechaSalida { get; set;  }
        public string? Banco { get; set; }
        public string? CuentaBancaria { get; set; }

    }
}
