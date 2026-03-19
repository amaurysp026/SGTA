using Microsoft.EntityFrameworkCore;
using SFCH.IService;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Logica
{
    public class ProveedorService : IProveedor
    {
        private Conexion db = new Conexion();
        public Task<bool> ActualizarProveedor(Proveedor proveedor)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GuardarProveedor(Proveedor proveedor)
        {
          await  db.Database.BeginTransactionAsync();
            try
            {
                db.Proveedores.Add(proveedor);
                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                await db.Database.RollbackTransactionAsync();
                return false;
            }

        }

        public async Task<List<Proveedor>> ObtenerProveedores()
        {
           db.ChangeTracker.Clear();
            return await db.Proveedores.Where(p => p.Activo).OrderDescending().AsNoTracking().ToListAsync();
        }
    }
}
