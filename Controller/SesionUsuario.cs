using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.Controller
{
    public static class SesionUsuario
    {     
        public static Usuario Usuario { get; set; } = new Usuario();
        public static Turno TurnoActual { get; set; } = new Turno();
        public static string InfConexion { get; set; } = string.Empty;
        public static Configuracion Configuracion { get; set; } = new Configuracion();
        public static ConfiguracionLocal ConfiguracionLocal { get; set; } = new ConfiguracionLocal();

        public static void Limpiar()
        {
            Usuario = new Usuario();
            ConfiguracionLocal = new ConfiguracionLocal();
            Configuracion = new Configuracion();
        }
    }
}
