using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.IService
{
    public interface IContabilidad
    {
        Task<List<CxC>> OptenerCxCs();
        Task<CxC> OptenerCxC(int id);
        Task<List<PagoCxC>> OptenerPagos(int cxcId);
         Task AgregarPago(PagoCxC pago);
         Task AnularPago(int pagoId);
         Task AnularCxC(int cxcId);
    }
}
