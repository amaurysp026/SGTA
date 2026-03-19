using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.IService
{
  public  interface IHerramientas
    {
        Task<bool> ComprobarConexionInternet();
        Task<bool> DescargarArchivo(string url, string rutaDestino);
        Task<bool> GuardarEntidad(TipoEntidad entidad);
        Task<List<TipoEntidad>> ObtenerEntidades();
        Task<List<TipoCuenta>> ObtenerTiposCuenta();
        Task<List<Categoria>> ObtenerCategorias();
        Task<bool> GuardarCategorias(Categoria categoria);
        Task<bool> GuardarActualizar(Categoria categoria);
        Task<bool> DesactivarCategoria(Categoria categoria);

    }
}
