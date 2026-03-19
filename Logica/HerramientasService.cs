using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Scaffolding;
using SFCH.IService;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SFCH.Logica
{
    public class HerramientasService : IHerramientas
    {
        private readonly Conexion db=new Conexion();
        public Task<bool> ComprobarConexionInternet()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DescargarArchivo(string url, string rutaDestino)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GuardarEntidad(TipoEntidad entidad)
        {
            try
            { var a=await ObtenerEntidades();
                if (a.Where(x => x.Nombre == entidad.Nombre).Count() >0)
                {
                    MessageBox.Show("La entidad ya existe.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                using (var db = new Conexion())
                {
                    await db.TipoEntidades.AddAsync(entidad);
                  if(  await db.SaveChangesAsync() > 0)
                    {
                        return true;

                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar la entidad.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                      
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la entidad: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<List<Categoria>> ObtenerCategorias()
        {
            return await db.Categorias.Include(x=>x.Productos).AsNoTracking().Where(x=>x.Activo).ToListAsync();
        }
        public async Task<bool> GuardarActualizar(Categoria categoria)
        { db.Database.BeginTransaction();
            try
            {
                if (categoria.Id > 0)
                {
                    db.Categorias.Update(categoria);
                }
                else
                {
                    await db.Categorias.AddAsync(categoria);
                }
                var x = await db.SaveChangesAsync() > 0;
                if (x)
                {
                    await db.Database.CommitTransactionAsync();
                    return true;
                }
                else
                {
                    await db.Database.RollbackTransactionAsync();
                    return false;
                }
            }
            catch (Exception ex)
            {
                await db.Database.RollbackTransactionAsync();
                MessageBox.Show("Error al guardar la categoría: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        

        }
        public async Task<bool> GuardarCategorias(Categoria categoria)
        {
             await  db.Database.BeginTransactionAsync();
            try
            {
                await db.Categorias.AddAsync(categoria);
                var x= await db.SaveChangesAsync() > 0;
                if (x)
                {
                    await db.Database.CommitTransactionAsync();
                    return true;
                }
                else
                {
                    await db.Database.RollbackTransactionAsync();
                    return false;
                }
            }
            catch (Exception ex)
            {
                await db.Database.RollbackTransactionAsync();
                MessageBox.Show("Error al guardar la categoría: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public async Task<List<TipoEntidad>> ObtenerEntidades()
        {
            try
            {
                using (var db = new Conexion())
                {
                    return await db.TipoEntidades.AsNoTracking().ToListAsync();
                }

            }
            catch (Exception ex)
            {
               MessageBox.Show("Error al obtener las entidades: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<TipoEntidad>();
            }
        }
        public async Task<List<TipoCuenta>> ObtenerTiposCuenta()
        {
            try
            {
                using (var db = new Conexion())
                {
                    return await db.TipoCuentas.AsNoTracking().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los tipos de cuenta: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<TipoCuenta>();
            }
        }

        public async Task<bool> DesactivarCategoria(Categoria categoria)
        {
            db.Database.BeginTransaction();
            try
            {
                if (categoria.Id > 0)
                {
                    categoria.Activo=false;
                    db.Categorias.Update(categoria);
                }
                else
                {
                    await db.Categorias.AddAsync(categoria);
                }
                var x = await db.SaveChangesAsync() > 0;
                if (x)
                {
                    await db.Database.CommitTransactionAsync();
                    return true;
                }
                else
                {
                    await db.Database.RollbackTransactionAsync();
                    return false;
                }
            }
            catch (Exception ex)
            {
                await db.Database.RollbackTransactionAsync();
                MessageBox.Show("Error al guardar la categoría: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
    }
}
