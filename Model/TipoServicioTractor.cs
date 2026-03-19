using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class TipoServicioTractor
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public bool Activo { get; set; }
    }
}
