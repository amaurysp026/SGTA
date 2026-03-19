using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? RNC { get; set; }
        public string? Direccion { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string? Telefono { get; set; } 
        public string? Contacto { get; set; }
        public string? Color { get; set; }

        public bool Activo { get; set; } = true;
       

        public override string ToString()
        {
            return Nombre;
        }
    }
}
