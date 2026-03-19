using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.IService
{
    public interface IProveedor
    {
        Task<bool> GuardarProveedor(Proveedor proveedor);
        Task<bool> ActualizarProveedor(Proveedor proveedor);
        Task<List<Proveedor>> ObtenerProveedores();
    }
}
