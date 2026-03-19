using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.IService
{
    public interface IUsuario
    {
        Task<bool> ValidarUsuarioAsync(Usuario usuario, string clave);
        Task<bool> CambiarClaveAsync(Usuario usuario, string nuevaClave);
        Task<bool> RegistrarUsuarioAsync(Usuario usuario);
        Task<bool> DesactivarUsuarioAsync(Usuario usuario);
        Task<List<Usuario>> ObtenerUsuariosAsync();
        Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);
        Task<Usuario?> ObtenerUsuarioPorNombreAsync(string nombreUsuario);
        Task<bool> EntradaConfiguracionLocal(string NombreUsuario,bool recoUser);
        Task<ConfiguracionLocal> OptenerConfiguracionLocal();
        Task<bool> ActualizarUsuario(Usuario usuario);
    }
}
