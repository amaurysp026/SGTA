using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.Model
{
    public class ConfiguracionLocal
    {
        public int Id { get; set; }
        public string NombreEquipo { get; set; } = string.Empty;
        public string PrintReporte { get; set; } = string.Empty;
        public string PrintFactura { get; set; } = string.Empty;
        public string PrintRecibo { get; set; } = string.Empty;
        public string PrintPuntoVenta { get; set; } = string.Empty;
        public string PrintValeCaja { get; set; } = string.Empty;
        public string UltimoUsuario { get; set; } = string.Empty;
        public DateTime UltimoInicio { get; set; } = DateTime.Now;
        public string Licencia { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public virtual Usuario usuario { get; set; } = new Usuario();
        public bool RecordarClave { get; set; } = false;
        public bool RecordarUsuario { get; set; } = false;
        public int UsosC { get; set; } = 0;
        public int UsosU { get; set; } = 0;
        public int inicio { get; set; } = 0;
        public string UsuarioRec { get; set; } = string.Empty;
        public string ClaveRec { get; set; } = string.Empty;
    }
}
