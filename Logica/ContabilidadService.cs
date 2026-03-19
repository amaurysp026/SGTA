using Microsoft.EntityFrameworkCore;
using SFCH.IService;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Logica
{
    public class ContabilidadService : IContabilidad
    {
        Conexion db= new Conexion();
        public Task AgregarPago(PagoCxC pago)
        {
            throw new NotImplementedException();
        }

        public Task AnularCxC(int cxcId)
        {
            throw new NotImplementedException();
        }

        public Task AnularPago(int pagoId)
        {
            throw new NotImplementedException();
        }

        public Task<CxC> OptenerCxC(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CxC>> OptenerCxCs()
        {
            return db.CxCs.Include(x=>x.Cliente).Include(x => x.Cliente2).Where(x=>x.Anulado==false).ToList();
        }

        public Task<List<PagoCxC>> OptenerPagos(int cxcId)
        {
            throw new NotImplementedException();
        }
    }
}
