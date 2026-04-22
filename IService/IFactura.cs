using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.IService
{
    public interface IFactura
    {
        Task<bool> GuardarFactura(Factura factura,bool sindetalle=false);
        Task<bool> ActualizarFactura(Factura factura);
        Task<List<Factura>> ObtenerFacturas();
        Task<bool> ImprimirFactura(Factura factura, bool Prev = false);
        Task<Factura?> ObtenerFacturaPorId(Factura factura);
        Task<bool> ImprimirFacturaGrande(Factura factura,bool Prev=false);
        Task<bool> AnularFactura(Factura factura);
    }
}
