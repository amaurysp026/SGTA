using MaterialDesignColors.Recommended;
using Microsoft.EntityFrameworkCore;
using SFCH.Controller;
using SFCH.IService;
using SFCH.Model;
using SFCH.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace SFCH.Logica
{
    public class CompraService : ICompra
    {
        private Conexion db = new Conexion();
        public Task<bool> ActualizarCompra(Compra compra)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AnularCompra(Compra compra)
        {
            if (compra == null || compra.Id <= 0)
            {
                MessageBox.Show("La compra no es válida");
                return false;
            }
            if (!SesionUsuario.Usuario.Administrador)
            {
                MessageBox.Show("No tiene permisos para anular compras, contacte al administrador");
                return false;

            }
            if (MessageBox.Show("¿Está seguro de anular esta compra?", "Confirmar Anulación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return false;
            }
            if (compra.Nula)
            {
                MessageBox.Show("La compra ya está anulada");
                return false;
            }
            if (compra.MontoPagado > 0)
            {
                MessageBox.Show("No se puede anular una compra con pagos realizados");
                return false;
            }
            await db.Database.BeginTransactionAsync();
            try
            {
                compra = await db.Compras
                    .Include(c => c.Detalles)
                    .ThenInclude(d => d.Producto)
                    .FirstOrDefaultAsync(c => c.Id == compra.Id) ?? new Compra();
                foreach (var detalle in compra.Detalles)
                {
                    var producto = await db.Productos.FindAsync(detalle.Producto.Id);
                    if (producto != null)
                    {
                        producto.Existencias.Add(new Existencia
                        {
                            CantidadTotal = -detalle.Cantidad,
                            FechaRegistro = DateTime.Now,
                            Descripcion = $"Anulación de compra N° {compra.NumeroCompra}",
                            Documento = "compra N°" + compra.NumeroFactura,

                        });

                        db.Productos.Update(producto);
                    }
                }
                compra.Nula = true;
                await db.Existencias.AddRangeAsync(compra.Detalles.Select(d => new Existencia
                {
                    Producto = d.Producto,
                    CantidadTotal = -d.Cantidad,
                    CostoUnitario = d.PrecioUnitario,
                    FechaRegistro = DateTime.Now,
                    Descripcion = $"Anulación de compra N° {compra.NumeroCompra}",
                    Documento = "compra N°" + compra.NumeroFactura

                }));

                db.Compras.Update(compra);
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

        public async Task<bool> GuardarCompra(Compra compra)
        {
            await db.Database.BeginTransactionAsync();
            try
            {
                if (compra.Proveedor == null)
                {
                    MessageBox.Show("El proveedor no puede ser nulo");
                    db.Database.RollbackTransaction();
                    return false;
                }
                if (compra.Usuario == null)
                {
                    MessageBox.Show("El usuario no puede ser nulo");
                    db.Database.RollbackTransaction();
                    return false;
                }
                compra.Proveedor = await db.Proveedores.FindAsync(compra.Proveedor.Id) ?? new Proveedor();
                compra.Usuario = await db.Usuarios.FindAsync(compra.Usuario.Id) ?? new Usuario();
            
                foreach (var detalle in compra.Detalles)
                {
                    var auxlotes = detalle.Producto.lotes;
                    detalle.Producto =await db.Productos.Include(x => x.Existencias!).FirstOrDefaultAsync(x => x.Id == detalle.Producto.Id);
                    foreach (var l in auxlotes)
                    {
                        l.Producto = detalle.Producto;
                        l.Usuario=compra.Usuario;
                    }

                    detalle.Producto?.lotes = auxlotes;
                    await db.Lotes.AddRangeAsync(detalle.Producto.lotes);
                    MessageBox.Show("Lote " + detalle.Producto.lotes.Count());
                    decimal cantidadactual = detalle.Producto.Existencias.Sum(x => x.Entrada) - detalle.Producto.Existencias.Sum(x => x.Salida);
                    if (detalle.Producto == null)
                    {
                        MessageBox.Show("El producto no existe en la base de datos");
                        await db.Database.RollbackTransactionAsync();
                        return false;
                    }
                    if (detalle.Producto.Existencias == null)
                    {
                        detalle.Producto.Existencias = new List<Existencia>();
                    }
                    if (detalle.Producto.lotes == null && detalle.Producto.Vence)
                    {
                        detalle.Producto.lotes = new List<Lote>();
                    }
                    detalle.Producto.Existencias.Add(new Existencia
                    {
                        CantidadTotal = cantidadactual + detalle.Cantidad,
                        CostoUnitario = detalle.PrecioUnitario,
                        FechaRegistro = DateTime.Now,
                        Descripcion = $"Compra N° {compra.NumeroCompra}",
                        Documento = "Factura Compra N°" + compra.NumeroFactura,
                        Entrada = detalle.Cantidad,
                        Salida = 0,
                        NombreUsuario = compra.Usuario.NombreCompleto
                    });

                   

                    detalle.Producto.Costo = detalle.PrecioUnitario;
                    //var nuevprecio=detalle.PrecioUnitario-detalle.Producto.Precio;
                    //if (nuevprecio < detalle.Producto.Precio)
                    //{
                    //    var res = MessageBox.Show($"El precio del producto {detalle.Producto.Nombre} es menor que el precio actual de venta, Desea Cambiar el precio de todos modos?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    //    if (res==DialogResult.Yes)
                    //    {
                    //       //MessageBox.Show($"El precio del producto {detalle.Producto.Nombre} se ha actualizado de {detalle.Producto.Precio} a {nuevprecio}");
                    //        detalle.Producto.Precio += nuevprecio;
                    //    }
                    //}
                    //else
                    //{
                    //    detalle.Producto.Precio += nuevprecio;
                    //
                    //}
                  await  db.DetalleCompras.AddAsync(detalle);
                }
             await   db.Compras.AddAsync(compra);

                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await db.Database.RollbackTransactionAsync();
                MessageBox.Show("Error al guardar la compra: " + ex.Message);
                // return false;
                throw;
            }
        }

        public Task<bool> ImprimirCompra(Compra compra)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Compra>> ObtenerCompras()
        {
            return await db.Compras.Where(x => x.Nula == false)
                .Include(c => c.Proveedor)
                .Include(c => c.Usuario)
                .Include(c => c.Detalles)
                .ThenInclude(d => d.Producto)
                .AsNoTracking().OrderDescending().ToListAsync();
        }
    }
}
