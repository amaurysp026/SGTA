using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.IService
{
    public interface ICategorias
    {
         Task<bool> GuardarCategoria(Categoria categoria);
        Task<ObservableCollection<Categoria>> ObtenerCategorias();
        Task<List<Producto>> ObtenerTodosProductos();
        Task<ObservableCollection<UnidadMedida>> ObtenerUnidad();
    }
}
