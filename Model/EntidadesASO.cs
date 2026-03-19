using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.Model
{
    public class EntidadesASO
    {
        public int ID { get; set; }
        public string? ENTIDAD { get; set; }
        public string CODIGO { get; set; }
        public string NOMBRE { get; set; }
        public string? CONTACTO { get; set; }
        public string? TELEFONO1 { get; set; }
        public string? TELEFONO2 { get; set; }
        public string? TELCONTACTO { get; set; }
        public string? RNC { get; set; }
        public string? CTABANC { get; set; }
        public string? DIRECCION { get; set; }
        public char? GENERO { get; set; }
        public DateTime? FECNAC { get; set; }
    }
}
