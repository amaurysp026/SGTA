using SFCH.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.Model
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Pantalla { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
        public string Detalles { get; set; } = string.Empty;
        public virtual Usuario Usuario { get; set; }=SesionUsuario.Usuario;
        public string Equipo { get;set; } = Environment.MachineName;
    }
}
