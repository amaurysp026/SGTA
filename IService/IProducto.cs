using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.IService
{
    public interface IProducto
    {
        Task<bool> GuardarProducto(Producto producto);
        Task<List<Producto>> ObtenerProductos();
        Task<List<Impuesto>> ObtenerImpuesto();
        Task<bool> ActualizarProducto(Producto producto);
        Task<List<Existencia>> OptenerExistencias();
        Task<List<Lote>> OptenerLotes();
        Task<bool> MovAjusteInventario(int Id, decimal Ajuste,string Mensaje);
        Task<List<ContInventario>> OptenerContInventario();
        Task<bool> AplicarInventario(int id);
        Task<bool>GuardarInventario(ContInventario cont);
    }
}
