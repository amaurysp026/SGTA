using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.IService
{
    public interface ICompra
    {
        Task<bool> GuardarCompra(Compra compra);
        Task<bool> ActualizarCompra(Compra compra);
        
        Task<List<Compra>> ObtenerCompras();
        Task<bool> ImprimirCompra(Compra compra);
        Task<bool> AnularCompra(Compra compra);

    }
}
