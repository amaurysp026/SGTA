using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.Model
{
    public class Impuesto
    {
        public int Id { get; set; }
        public string NombreImpuesto { get; set; } = null!;
        public decimal Porcentaje { get; set; }
    }
}
