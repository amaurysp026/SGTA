using Microsoft.EntityFrameworkCore;
using SFCH.Controller;
using SFCH.IService;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SFCH.Logica
{
    public class ProductoService : IProducto
    {
        Conexion db= new Conexion();

        public async Task<bool> ActualizarProducto(Producto producto)
        {
            await db.Database.BeginTransactionAsync();
            try
            {
                if (producto.Unidad == null || producto.Categoria == null)
                {
                    MessageBox.Show("Algunos Campos Son obligatorios");
                    return false;
                }
                producto.Unidad = await db.UnidadMedidas.FirstOrDefaultAsync(u => u.Id == producto.Unidad.Id);
                producto.Categoria = await db.Categorias.FirstOrDefaultAsync(c => c.Id == producto.Categoria.Id);
                db.Productos.Update(producto);
                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await db.Database.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task<bool> AplicarInventario(int id)
        {
            await db.Database.BeginTransactionAsync();
            try
            {
                ContInventario inventario= db.ContInventarios.Include(x=>x.Detalle).ThenInclude(x=>x.Producto).FirstOrDefault(x => x.Id == id)??null;
               if (inventario == null)
                {
                    MessageBox.Show("Inventario no encontrado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    await db.Database.RollbackTransactionAsync();
                    return false;
                }
               foreach (var item in inventario.Detalle)
                {
                    var producto = await db.Productos
                    .Include(p => p.Existencias)
                    .FirstOrDefaultAsync(p => p.Id == item.Producto.Id);
                    if (producto == null)
                    {
                        MessageBox.Show($"Producto con ID {item.Producto.Id} no encontrado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        await db.Database.RollbackTransactionAsync();
                        return false;
                    }
                    // Asegurar la colección de existencias
                    if (producto.Existencias == null)
                        producto.Existencias = new System.Collections.Generic.List<Existencia>();
                    if(item.CantFisico==item.CantSistema)
                    {
                        continue; // No hay diferencia, no se registra movimiento
                    }
                    if (item.CantFisico==0)
                    {
                        // Registrar una salida por la totalidad de la existencia disponible para dejar el producto en 0
                        var salidaCantidad = producto?.CantidadDisponible ?? 0m;
                        if (salidaCantidad > 0)
                        {
                            var existenciaCero = new Existencia
                            {
                                CantidadTotal = 0,
                                CostoUnitario = producto?.Costo ?? 0m,
                                PrecioUnitario = producto?.Precio ?? 0m,
                                Descripcion = $"Aplicación de Inventario - Poner a 0 - ContInventario ID: {inventario.Id}",
                                Documento = $"ContInventario - {inventario.Id}",
                                Entrada = 0,
                                Salida = salidaCantidad,
                                FechaRegistro = DateTime.Now,
                                Producto = producto
                            };

                            producto.Existencias.Add(existenciaCero);
                            await db.Existencias.AddAsync(existenciaCero);
                        }
                        else
                        {
                            var existenciaCero = new Existencia
                            {
                                CantidadTotal = 0,
                                CostoUnitario = producto?.Costo ?? 0m,
                                PrecioUnitario = producto?.Precio ?? 0m,
                                Descripcion = $"Aplicación de Inventario - Poner a 0 - ContInventario ID: {inventario.Id}",
                                Documento = $"ContInventario - {inventario.Id}",
                                Entrada = Math.Abs( salidaCantidad),
                                Salida =0 ,
                                FechaRegistro = DateTime.Now,
                                Producto = producto
                            };

                            producto.Existencias.Add(existenciaCero);
                            await db.Existencias.AddAsync(existenciaCero);
                        }
                        continue;
                    }


                    var existencia = new Existencia
                    {
                        
                        CostoUnitario = producto?.Costo ?? 0m,
                        PrecioUnitario = producto?.Precio ?? 0m,
                        Descripcion = $"Aplicación de Inventario - ContInventario ID: {inventario.Id}",
                        Documento = $"ContInventario - {inventario.Id}",
                        Entrada = item.CantFisico > producto?.CantidadDisponible ? item.DiferenciaCalculada : 0,
                        Salida = item.CantFisico < producto?.CantidadDisponible ? Math.Abs( item.DiferenciaCalculada) : 0,
                        FechaRegistro = DateTime.Now,
                        Producto = producto
                      
                    };
                    producto.Existencias.Add(existencia);
                    await db.Existencias.AddAsync(existencia);
                }
               inventario.Aplicado = true;
                inventario.Estado = "Aplicado";
              //  inventario.FechaAplicacion = DateTime.Now;
                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar ajuste de inventario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await db.Database.RollbackTransactionAsync();
                throw ;
            }
        }

        public async Task<bool> GuardarInventario(ContInventario cont)
        {
            await db.Database.BeginTransactionAsync();
            try
            {
                if (cont == null || cont.Detalle == null || !cont.Detalle.Any())
                {
                    MessageBox.Show("El conteo de inventario y sus detalles son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    await db.Database.RollbackTransactionAsync();
                    return false;
                }

                // Inicializar campos del encabezado
                cont.Fecha = DateTime.Now;
                cont.Aplicado = false;
                cont.Estado = "Pendiente";
                cont.Usuario= await db.Usuarios.FirstOrDefaultAsync(x => x.Id == SesionUsuario.Usuario.Id) ?? new Usuario();
                // Asegurar que cada detalle referencia al producto existente en contexto
                foreach (var item in cont.Detalle)
                {
                    if (item == null || item.Producto == null || item.Producto.Id == 0)
                    {
                        MessageBox.Show("Cada detalle debe contener un producto válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        await db.Database.RollbackTransactionAsync();
                        return false;
                    }

                    var producto = await db.Productos.FirstOrDefaultAsync(p => p.Id == item.Producto.Id);
                    if (producto == null)
                    {
                        MessageBox.Show($"Producto con ID {item.Producto.Id} no encontrado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        await db.Database.RollbackTransactionAsync();
                        return false;
                    }

                    // Enlazar el producto rastreado por el contexto para evitar insertar uno nuevo
                    item.Producto = producto;

                    // Opcional: si existe una propiedad de referencia al encabezado, EF la gestionará al agregar el cont.
                    // item.ContInventario = cont; // descomentar si la clase detalle tiene esta propiedad
                }

                await db.ContInventarios.AddAsync(cont);
                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el conteo de inventario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> GuardarProducto(Producto producto)
        {
          await  db.Database.BeginTransactionAsync();
            try
            {
                if (producto.Unidad==null||producto.Categoria==null)
                {
                    MessageBox.Show("Algunos Campos Son obligatorios");
                    db.Database.RollbackTransaction();
                    return false;
                }
              //  producto.Existencias = new List<Existencia>();
                producto.Existencias.Add(new Existencia
                {
                    CantidadTotal = 0,
                    CostoUnitario = producto.Costo,
                    Descripcion = "Existencia Inicial",
                    Documento = "Creación del Producto "+producto.Nombre,
                    Entrada = 0,
                    FechaRegistro = DateTime.Now,
                    PrecioUnitario = producto.Precio,
                    Producto = producto,
                    Salida = 0
                    
                });
                producto.Unidad= await db.UnidadMedidas.FirstOrDefaultAsync(u => u.Id == producto.Unidad.Id);
                producto.Categoria= await db.Categorias.FirstOrDefaultAsync(c => c.Id == producto.Categoria.Id);
                
                await db.Productos.AddAsync(producto);
                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> MovAjusteInventario(int Id, decimal Ajuste, string Mensaje)
        {
            await db.Database.BeginTransactionAsync();
            try
            {
                var producto = await db.Productos
                    .Include(p => p.Existencias)
                    .FirstOrDefaultAsync(p => p.Id == Id);

                if (producto == null)
                {
                    MessageBox.Show("Producto no encontrado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    await db.Database.RollbackTransactionAsync();
                    return false;
                }

                // Asegurar la colección de existencias
                if (producto.Existencias == null)
                    producto.Existencias = new System.Collections.Generic.List<Existencia>();

                var existencia = new Existencia
                {
                    CantidadTotal = Ajuste,
                    CostoUnitario = producto?.Costo ?? 0m,
                    PrecioUnitario = producto?.Precio ?? 0m,
                    Descripcion = Mensaje + " - Movimiento de ajuste",
                    Documento = "Ajuste Inventario - " + Mensaje,
                    Entrada = Ajuste > 0 ? Ajuste : 0,
                    Salida = Ajuste < 0 ? Ajuste : 0,
                    FechaRegistro = DateTime.Now,
                    Producto = producto
                };

                producto.Existencias.Add(existencia);
                await db.Existencias.AddAsync(existencia);

                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar ajuste de inventario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await db.Database.RollbackTransactionAsync();
                return false ;
            }
           

        }

        public async Task<List<Impuesto>> ObtenerImpuesto()
        {
        return  await db.Impuestos.ToListAsync();
        }

        public async Task<List<Producto>> ObtenerProductos()
        {
            try
            {
              return db.Productos.Include(p => p.Categoria).Include(x=>x.Existencias).Include(p => p.Unidad).Where(p => p.Activo).OrderDescending().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ContInventario>> OptenerContInventario()
        {
            try
            {
                return await db.ContInventarios.Include(x => x.Detalle).ThenInclude(x => x.Producto).AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al cargar los conteos de inventario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<ContInventario>() ;
            }
            
        }

        public async Task<List<Existencia>> OptenerExistencias()
        {
            return await db.Existencias.Include(x => x.Producto).Include(x => x.Producto).ToListAsync();
        }

        public async Task<List<Lote>> OptenerLotes()
        {
            return await db.Lotes.Include(x => x.Producto).ThenInclude(x=>x.Unidad).Include(x => x.Usuario).ToListAsync();
            //.Where(x=>x.Cantidad>0)
        }
    }
}
