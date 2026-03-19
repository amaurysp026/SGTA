using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.Model
{
   public class TipoEntidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = string.Empty;
        public ICollection<Persona> Personas { get; set; } = new List<Persona>();
        public override string ToString()
        {
            return $"{Nombre}";
        }
    }
}
