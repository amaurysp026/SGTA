using Microsoft.EntityFrameworkCore;
using SFCH.IService;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Logica
{
    public class EntidadesService : IEntidad
    {
        public async Task<bool> GuardarEntidadesAsync(Entidad entidad)
        {
            using (var db=new Conexion())
            {
              await  db.Database.BeginTransactionAsync();
                try
                {
                    if(entidad.TipoEntidadAso!=null)
                        entidad.TipoEntidadAso=db.EntidadesASOs.Find(entidad.TipoEntidadAso.Id);
                    
                    db.Entidad.Add(entidad);

                    
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
        }

        public async Task<List<Entidad>> ObtenerEntidadesAsync()
        {
            
              using (var db=new Conexion())
            {
                return await  db.Entidad.Include(x=>x.TipoEntidadAso).AsNoTracking().ToListAsync();
            }
        }
    }
}
