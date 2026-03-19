using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.IService
{
   public interface IPersona
    {
        Task<bool> GuardarPersona(Persona persona);
        Task<List<Persona>> ObtenerPersonas();
        Task<bool> EditarPersonasAsync(Persona persona);
        Task<List<EntidadesASO>> ObtenerPersonasASO();
        Task<List<Persona>> ObtenerPersonas(TipoEntidad tipoEntidad);
        Task<bool> EliminarPersona(int id);
        Task<Persona> ObtenerPersonaPorId(int id);
        Task<Persona> ObtenerPersonaPorCedula(string t);
    }
}
