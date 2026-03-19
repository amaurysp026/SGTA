using Microsoft.EntityFrameworkCore;
using SFCH.IService;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SFCH.Logica
{
    public class CategoriaService : ICategorias

    {
        public async Task<bool> GuardarCategoria(Categoria categoria)
        {
            
                using (var db=new Conexion())
                {
                    try
                    {
                  await  db.Database.BeginTransactionAsync();
                    db.Categorias.Add(categoria);
                  await   db.SaveChangesAsync();
                    return true;
                }
                    catch (Exception)
                    {
                 await   db.Database.RollbackTransactionAsync();
                    MessageBox.Show("Error al guardar la categoria");
                    return false;
                }
                    

                }
           
        }

        public async Task<ObservableCollection<Categoria>> ObtenerCategorias()
        {
           
            using (var db=new Conexion())
            {
               
                var categorias = await db.Categorias.Where(c => c.Activo).Include(x => x.Productos).ThenInclude(x=>x.Unidad).AsNoTracking().ToListAsync();
                //var categorias = db.Categorias.Where(c => c.Activo).Include(x=>x.Productos).ToList();
                return new ObservableCollection<Categoria>(categorias);
            }
        }

        public async Task<List<Producto>> ObtenerTodosProductos()
        {
            try
            {
                using (var db = new Conexion())
                {
                    return db.Productos.Include(p => p.Categoria).Include(p => p.Unidad).Where(p => p.Activo).AsNoTracking().ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<ObservableCollection<UnidadMedida>> ObtenerUnidad()
        {

            using (var db = new Conexion())
            {

                var categorias = await db.UnidadMedidas.AsNoTracking().ToListAsync();
                //var categorias = db.Categorias.Where(c => c.Activo).Include(x=>x.Productos).ToList();
                return new ObservableCollection<UnidadMedida>(categorias);
            }
        }
    }
}
