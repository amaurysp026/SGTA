using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.IService
{
    public interface IEntidad
    {
        Task<List<Entidad>> ObtenerEntidadesAsync();
        Task<bool> GuardarEntidadesAsync(Entidad entidad);
    }
}
