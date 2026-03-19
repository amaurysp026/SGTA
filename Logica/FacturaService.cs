using Microsoft.EntityFrameworkCore;
using SFCH.Controller;
using SFCH.IService;
using SFCH.Model;
using SFCH.PrintView;
using SFCH.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Azure.Core.HttpHeader;
using static System.Collections.Specialized.BitVector32;

namespace SFCH.Logica
{
    public class FacturaService : IFactura
    {
        Conexion db = new Conexion();

        public async Task<bool> GuardarFactura(Factura factura)
        {
            if (factura.TotalPendiente>0&& factura.ClienteP == null)
            {
                MessageBox.Show("No es posible el credito a Cliente Genérico", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }
            if (factura.Detalles.Count() < 1 || factura.Total < 0)
            {
                MessageBox.Show("No es posible guardar una factura sin elementos", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (factura.Total <= 0)
            {
                MessageBox.Show("El monto de la factura es irregular y no se puede guardar", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Error);
                await db.Database.RollbackTransactionAsync();
                return false;
            }
            var cxc=new CxC();
            await db.Database.BeginTransactionAsync();
            try
            {
                if (factura.Id == 0)
                {
                   
                    factura.Turno = await db.Turnos.FindAsync(SesionUsuario.TurnoActual.Id);
                    factura.ClienteP = await db.Personas.Include(x => x.TipoEntidad).FirstOrDefaultAsync(x => x.Id == factura.ClienteP.Id);
                   
                    
                    if (!factura.Abierta)
                    {
                        if (factura.Efectivo > factura.Total)
                        {
                            factura.Efectivo = factura.Total;
                        }
                        if (factura.TotalPendiente>0)
                        {
                           

                            cxc = new CxC
                            {
                                //  Factura = factura,
                                //db.Attach(factura.Cliente);
                                Cliente = factura.ClienteP ,//db.Attach(factura.Cliente) //db.Entry(factura.Cliente).State = EntityState.Unchanged;
                                // Cliente= await db.Personas.Include(x=>x.TipoEntidad).FirstOrDefaultAsync(x=>x.Id==factura.ClienteP.Id),
                                MontoFactura = factura.Total,
                                MontoPendiente = factura.TotalPendiente,
                                MontoTotal = factura.Total,
                                Titular = factura.Cliente?.NombreCompleto,
                                Descripcion = "Generado por Factura a Credito",
                             //   Cliente2 = factura.Cliente,
                                DiasCredito = factura.DiasCredito == 0 ? factura.DiasCredito : 30

                            };
                            db.CxCs.Add(cxc);
                        }
                        foreach (var item in factura.Detalles)
                        {
                            if (item.Producto == null)
                            {
                                MessageBox.Show("El producto es nulo ");
                                
                                throw new Exception("El producto es nulo en la función");
                            }
                            var p = await db.Productos.Include(x => x.Existencias).FirstOrDefaultAsync(x => x.Id == item.Producto.Id);
                                var exactual = p.Existencias.Sum(x => x.Entrada) - p.Existencias.Sum(x => x.Salida);

                            if (!p.EsServicio)
                            {
                                if (!SesionUsuario.Configuracion.VenderSinExistencia)
                                {
                                    if (!p.PermitirStockNegativo)
                                    {

                                        if (exactual < item.Cantidad)
                                        {
                                            MessageBox.Show("No hay suficiente en inventario del producto: " + p.Nombre, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                            await db.Database.RollbackTransactionAsync();
                                            return false;
                                        }
                                    }
                                }

                                p.Existencias.Add(new Existencia
                                {
                                    CantidadTotal = exactual - item.Cantidad,
                                    FechaRegistro = DateTime.Now,
                                    Descripcion = "Venta factura N° " + factura.Id,
                                    Documento = "Factura N° " + factura.Id,
                                    Entrada = 0,
                                    Salida = item.Cantidad,
                                    CostoUnitario = item.CostoUnitario,
                                    PrecioUnitario = item.PrecioUnitario,
                                    NombreUsuario = SesionUsuario.Usuario.NombreCompleto

                                });
                                if (p.Vence)
                                {
                                    var lotes = await db.Lotes.Where(x => x.Cantidad > 0 && x.Producto.Id == p.Id).ToListAsync();
                                    if (lotes.Count() > 0)
                                    {
                                        Lote ellote;
                                        if (lotes.Count() > 1)
                                        {
                                            VLoteVenta vLoteVenta = new VLoteVenta(lotes);
                                            vLoteVenta.ShowDialog();
                                            var nl = vLoteVenta.codlote;
                                            ellote = lotes.FirstOrDefault(x => x.CodigoLote == nl);
                                            if (ellote.FechaVencimiento > lotes.OrderByDescending(x => x.FechaVencimiento).Last().FechaVencimiento)
                                            {
                                                if (MessageBox.Show("Del producto " + p.Nombre + " existe una fecha de vencimiento menor a la del lote indicado. Desea continuar de todos modos?", "Atención", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                                                {
                                                    await db.Database.RollbackTransactionAsync();
                                                    return false;
                                                }
                                            }
                                            //Trabajar esta paryte para que reste de varios lotes si el lote seleccionado no tiene suficiente cantidad
                                            ellote.Cantidad -= item.Cantidad;
                                            if (ellote.Cantidad > item.Cantidad && lotes.Count() > 2)
                                            {

                                            }
                                        }
                                        else
                                        {
                                            ellote = lotes.FirstOrDefault();
                                            if (ellote != null)
                                            {
                                                ellote.Cantidad -= item.Cantidad;
                                            }
                                        }
                                    }

                                }

                            }
                            else
                            {
                                p.Existencias.Add(new Existencia
                                {
                                    CantidadTotal = exactual - item.Cantidad,
                                    FechaRegistro = DateTime.Now,
                                    Descripcion = "Venta factura N° " + factura.Id,
                                    Documento = "Factura N° " + factura.Id,
                                    Entrada = 0,
                                    Salida = item.Cantidad,
                                    CostoUnitario = item.CostoUnitario,
                                    PrecioUnitario = item.PrecioUnitario,
                                    NombreUsuario = SesionUsuario.Usuario.NombreCompleto

                                });
                            }

                            item.Producto = p;

                        }

                        await db.Existencias.AddRangeAsync(factura.Detalles.Select(d => d.Producto?.Existencias?.Last()).Where(x => x != null)!);

                    }
                    else
                    {
                        foreach (var item in factura.Detalles)
                        {
                            if (item.Producto==null)
                            {
                                MessageBox.Show("El producto " + item.NombreProducto + " aun no esta listo para facturar, es necesario reiniciar facturar", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Warning);
                              await  db.Database.RollbackTransactionAsync();
                                return false;
                            }
                            item.Producto = await db.Productos.FindAsync(item.Producto?.Id);
                        }
                    }
                    await db.CxCs.AddAsync(cxc);

                    await db.Facturas.AddAsync(factura);
                    await db.SaveChangesAsync();
                    if (factura.Id != 0 &&factura.TotalPendiente>0)
                    {
                     
                       
                    }
                        await db.Database.CommitTransactionAsync();
                    return true;
                }
                else
                {
                    await db.Database.RollbackTransactionAsync();
                    MessageBox.Show("La factura ya existe y no se puede guardar de nuevo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la factura: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                await db.Database.RollbackTransactionAsync();
                throw;
            }
        }
        private void AddCell(TableRow row, string text, TextAlignment alignment = TextAlignment.Left)
        {
            var paragraph = new Paragraph(new Run(text)) { TextAlignment = alignment };
            TableCell cell = new TableCell(paragraph);
            cell.BorderBrush = Brushes.Black;
            cell.BorderThickness = new Thickness(1);
            cell.Padding = new Thickness(5);
            row.Cells.Add(cell);
        }
        private void AddCellNul(TableRow row, string text, TextAlignment alignment = TextAlignment.Left)
        {
            var paragraph = new Paragraph(new Run(text)) { TextAlignment = alignment };
            TableCell cell = new TableCell(paragraph);
            cell.FontSize = 12;
            //cell.BorderBrush = System.Windows.Media.Brushes.Black;
            //cell.BorderThickness = new Thickness(0.5, 0.5, 0, 0.5);
            cell.Padding = new Thickness(0);

            row.Cells.Add(cell);
        }
        private void AddCellNul2(TableRow row, string text, TextAlignment alignment = TextAlignment.Left)
        {
            var paragraph = new Paragraph(new Run(text)) { TextAlignment = alignment };
            TableCell cell = new TableCell(paragraph);
            // cell.Background = Brushes.LightGray;
            //     cell.FontWeight = FontWeights.Bold;
            cell.BorderBrush = Brushes.Black;
            cell.FontFamily = new FontFamily("Calibri");
            //  cell.FontWeight = FontWeights.Bold;
            cell.FontSize = 12;
            cell.BorderThickness = new Thickness(0, 0.5, 0, 0.5);
            //  cell.BorderThickness = new Thickness(1);
            //  cell.Padding = new Thickness(5);
            row.Cells.Add(cell);
        }
        private void AddCellNul3(TableRow row, string text, TextAlignment alignment = TextAlignment.Left)
        {
            var paragraph = new Paragraph(new Run(text)) { TextAlignment = alignment };
            TableCell cell = new TableCell(paragraph);
            // cell.Background = Brushes.LightGray;
            //     cell.FontWeight = FontWeights.Bold;
            cell.BorderBrush = Brushes.Black;
            cell.FontFamily = new FontFamily("Calibri");
            //  cell.FontWeight = FontWeights.Bold;
            cell.FontSize = 12;
            cell.BorderThickness = new Thickness(0, 0.5, 0, 0);
            //  cell.BorderThickness = new Thickness(1);
            //  cell.Padding = new Thickness(5);
            row.Cells.Add(cell);
        }
        private void AddCellT(TableRow row, string text, TextAlignment alignment = TextAlignment.Left)
        {
            var paragraph = new Paragraph(new Run(text)) { TextAlignment = alignment };
            TableCell cell = new TableCell(paragraph);
            cell.BorderBrush = System.Windows.Media.Brushes.Black;
            cell.BorderThickness = new Thickness(0.5);
            cell.FontWeight = FontWeights.Bold;
            cell.FontSize = 16;
            cell.Padding = new Thickness(5);
            cell.Padding = new Thickness(5, 0, 0, 0);
            row.Cells.Add(cell);
        }
        public async Task<bool> ImprimirFactura(Factura factura,bool Prev=false)
        {

            try
            {
                // Crear el documento de impresión
                FlowDocument doc = new FlowDocument();
                doc.PageHeight = Double.NaN; // dejar que la altura se ajuste automáticamente al contenido
                doc.PageWidth = 275;
                doc.PagePadding = new Thickness(2);
                //  doc.ColumnWidth = 816;
                doc.TextAlignment = TextAlignment.Center; // Centrar todo el contenido del documento

                Paragraph NombreComercial = new Paragraph(new Run(SesionUsuario.Configuracion.NombreEmpresa));
                NombreComercial.FontSize = 24;
                NombreComercial.FontFamily = new FontFamily("Arial Black");
                NombreComercial.TextAlignment = TextAlignment.Center;
                NombreComercial.FontWeight = FontWeights.ExtraBold;
                NombreComercial.Margin = new Thickness(0, 0, 0, 2);

                // Añadir título
                Paragraph direccion = new Paragraph(new Run(SesionUsuario.Configuracion.Direccion));
                direccion.FontSize = 12;
                direccion.FontFamily = new FontFamily("Calibri");
                direccion.TextAlignment = TextAlignment.Center;
                direccion.Margin = new Thickness(0, 0, 0, 1);

                Paragraph Tels = new Paragraph(new Run("Tel.:" + SesionUsuario.Configuracion.Telefono));
                Tels.FontSize = 12;
                Tels.FontFamily = new FontFamily("Calibri");
                Tels.TextAlignment = TextAlignment.Center;
                Tels.Margin = new Thickness(0, 0, 0, 1);

                // Añadir título
                Paragraph RNC = new Paragraph(new Run("RNC-" + SesionUsuario.Configuracion.RNC));
                RNC.FontSize = 12;
                RNC.TextAlignment = TextAlignment.Center;
                RNC.FontFamily = new FontFamily("Calibri");
                RNC.FontWeight = FontWeights.Bold;
                RNC.Margin = new Thickness(0, 0, 0, 1);
                //var STR=DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                Paragraph title = new Paragraph(new Run("Factura No." + factura.Id));
                title.FontSize = 14;
                title.FontFamily = new FontFamily("Calibri");
                title.TextAlignment = TextAlignment.Center;
                title.FontWeight = FontWeights.Bold;
                title.Margin = new Thickness(0, 0, 0, 2);

                Paragraph fecha = new Paragraph(new Run(factura.FechaEmision.ToString(SesionUsuario.Configuracion.FormatoFecha + " - " + SesionUsuario.Configuracion.FormatoHora)));
                fecha.FontSize = 12;
                fecha.FontFamily = new FontFamily("Calibri");
                fecha.TextAlignment = TextAlignment.Right;
                fecha.Margin = new Thickness(0, 0, 0, 0);

                Paragraph condi = new Paragraph(new Run("Condición: " + factura.Condicion));
                condi.FontSize = 12;
                condi.FontFamily = new FontFamily("Calibri");
                condi.TextAlignment = TextAlignment.Left;
                condi.Margin = new Thickness(0, 0, 0, 2);

                Paragraph subtitle = new Paragraph(new Run("Cliente: " + factura.NombreCliente));
                subtitle.FontSize = 12;
                subtitle.FontFamily = new FontFamily("Calibri");
                subtitle.TextAlignment = TextAlignment.Left;
                subtitle.Margin = new Thickness(0, 0, 0, 0);
                Paragraph TelCliente = new Paragraph(new Run("Tel.:" + factura.TelefonoCliente));
                TelCliente.FontSize = 12;
                TelCliente.FontFamily = new FontFamily("Calibri");
                TelCliente.TextAlignment = TextAlignment.Left;
                TelCliente.Margin = new Thickness(0, 0, 0, 2);

                // doc.Blocks.Add(title);
                doc.Blocks.Add(NombreComercial);
                doc.Blocks.Add(direccion);
                doc.Blocks.Add(Tels);
                doc.Blocks.Add(RNC);
                doc.Blocks.Add(title);
                doc.Blocks.Add(fecha);
                doc.Blocks.Add(condi);
                doc.Blocks.Add(subtitle);


                // Crear tabla
                Table table = new Table();
                table.CellSpacing = 0;
                table.FontFamily = new FontFamily("Calibri");
                table.Padding = new Thickness(0); // Eliminar relleno
                                                  // table.Margin = new Thickness(170, 0, 0, 0); // Eliminar márgenes
                table.TextAlignment = TextAlignment.Center;
                // Definir columnas con anchos específicos
                table.Columns.Add(new TableColumn() { Width = new GridLength() });
                table.Columns.Add(new TableColumn() { Width = new GridLength(50) });
                table.Columns.Add(new TableColumn() { Width = new GridLength(50) });
                table.Columns.Add(new TableColumn() { Width = new GridLength(50) });
                table.Columns.Add(new TableColumn() { Width = new GridLength(50) });



                // Crear encabezados
                TableRowGroup headerGroup = new TableRowGroup();
                TableRow headerRow = new TableRow();
                string[] heders = { "Producto", "Cant", "Precio", "ITBIS", "Total" };
                foreach (string column in heders)
                {
                    TableCell cell = new TableCell(new Paragraph(new Run(column.ToString().ToUpper())));
                    // cell.Background = Brushes.LightGray;
                    //     cell.FontWeight = FontWeights.Bold;
                    cell.BorderBrush = Brushes.Black;
                    cell.FontFamily = new FontFamily("Calibri");
                    cell.FontWeight = FontWeights.Bold;
                    cell.FontSize = 12;
                    cell.BorderThickness = new Thickness(0, 0.5, 0, 0.5);
                    //  cell.BorderThickness = new Thickness(1);
                    cell.Padding = new Thickness(3);
                    headerRow.Cells.Add(cell);
                }
                headerGroup.Rows.Add(headerRow);
                table.RowGroups.Add(headerGroup);
                //    FontFamily fuente = new FontFamily("Arial");
                // Añadir datos
                TableRowGroup dataGroup = new TableRowGroup();
                foreach (var item in factura.Detalles)
                {


                    TableRow row2 = new TableRow();

                    // Crear una celda que abarque todas las columnas para mostrar el nombre del producto centrado
                    var paragraph = new Paragraph(new Run(item.NombreProducto.ToUpperInvariant())) { TextAlignment = TextAlignment.Left };
                    TableCell fullCell = new TableCell(paragraph)
                    {
                        ColumnSpan = table.Columns.Count,
                        FontSize = 12
                        //   BorderBrush = Brushes.Black,
                        //   BorderThickness = new Thickness(1),
                        // Padding = new Thickness(5)
                    };
                    row2.Cells.Add(fullCell);

                    dataGroup.Rows.Add(row2);
                    TableRow row = new TableRow();

                    // row.FontSize = 14;
                    // Añadir las celdas según las propiedades visibles en el DataGrid
                    AddCellNul(row, "", TextAlignment.Center);
                    AddCellNul(row, item.Cantidad.ToString("N"), TextAlignment.Center);
                    AddCellNul(row, item.PrecioUnitario.ToString("N"), TextAlignment.Center);
                    AddCellNul(row, item.ITBIS.ToString("N"), TextAlignment.Center);
                    //  AddCellNul(row, item.Descuento.ToString("N"), TextAlignment.Left);
                    AddCellNul(row, item.Total.ToString("N"), TextAlignment.Right); // Alineación derecha para el monto
                    dataGroup.Rows.Add(row);
                }



                table.RowGroups.Add(dataGroup);

                table.TextAlignment = TextAlignment.Center;
                doc.Blocks.Add(table);
                ///
                // Crear tabla
                Table tablet = new Table();
                tablet.CellSpacing = 0;
                tablet.FontFamily = new FontFamily("Calibri");
                tablet.Padding = new Thickness(0); // Eliminar relleno
                table.Margin = new Thickness(0, 0, 0, 0); // Eliminar márgenes
                tablet.TextAlignment = TextAlignment.Center;
                // Definir columnas con anchos específicos

                tablet.Columns.Add(new TableColumn() { Width = new GridLength() });
                tablet.Columns.Add(new TableColumn() { Width = new GridLength(100) });
                tablet.Columns.Add(new TableColumn() { Width = new GridLength() });

                TableRowGroup dataGroupT = new TableRowGroup();
                var xt = new TableRow();
                AddCellNul3(xt, "", TextAlignment.Right);
                AddCellNul3(xt, "SUB-TOTAL:", TextAlignment.Right);
                AddCellNul3(xt, factura.SubTotal.ToString("C"), TextAlignment.Right);
                dataGroupT.Rows.Add(xt);
                var x1t = new TableRow();
                AddCellNul(x1t, "", TextAlignment.Right);
                AddCellNul(x1t, "TOTAL ITBIS:", TextAlignment.Right);
                AddCellNul(x1t, factura.ITBIS.ToString("C"), TextAlignment.Right);
                dataGroupT.Rows.Add(x1t);
                var x2t = new TableRow();
                AddCellNul(x2t, "", TextAlignment.Right);
                AddCellNul(x2t, "TOTAL GENERAL:", TextAlignment.Right);
                AddCellNul(x2t, factura.Total.ToString("C"), TextAlignment.Right);
                dataGroupT.Rows.Add(x2t);
                if (factura.Efectivo > 0)
                {
                    var x2tp = new TableRow();
                    AddCellNul(x2tp, "", TextAlignment.Right);
                    AddCellNul(x2tp, "EFECTIVO:", TextAlignment.Right);
                    AddCellNul(x2tp, factura.Efectivo.ToString("C"), TextAlignment.Right);
                    dataGroupT.Rows.Add(x2tp);
                    var x3 = new TableRow();
                    AddCellNul(x3, "", TextAlignment.Right);
                    AddCellNul(x3, "PAGO CON:", TextAlignment.Right);
                    AddCellNul(x3, factura.PagoCon.ToString("C"), TextAlignment.Right);
                    dataGroupT.Rows.Add(x3);

                    var x2tpC = new TableRow();
                    AddCellNul(x2tpC, "", TextAlignment.Right);
                    AddCellNul(x2tpC, "CAMBIO:", TextAlignment.Right);
                    AddCellNul(x2tpC, factura.Cambio.ToString("C"), TextAlignment.Right);
                    dataGroupT.Rows.Add(x2tpC);
                }
                var x2tpCP = new TableRow();
                AddCellNul(x2tpCP, "", TextAlignment.Right);
                AddCellNul(x2tpCP, "TOTAL PENDIENTE:", TextAlignment.Right);
                AddCellNul(x2tpCP, factura.TotalPendiente.ToString("C"), TextAlignment.Right);
                dataGroupT.Rows.Add(x2tpCP);
                tablet.RowGroups.Add(dataGroupT);

                tablet.TextAlignment = TextAlignment.Center;
                doc.Blocks.Add(tablet);

                Paragraph vendedor = new Paragraph(new Run("Le  Atendio:" + factura.NombreVendedor));
                vendedor.FontSize = 12;
                vendedor.FontFamily = new FontFamily("Calibri");
                vendedor.TextAlignment = TextAlignment.Left;
                vendedor.Margin = new Thickness(0, 0, 0, 0);

                doc.Blocks.Add(vendedor);


                // Añadir total al final !! Gracias Por Su Compra Y Feliz Navidad !!
                Paragraph total = new Paragraph(new Run(SesionUsuario.Configuracion.MesnajeFinal1));
                total.FontSize = 12;
                // total.FontWeight = FontWeights;
                total.TextAlignment = TextAlignment.Center;
                total.Margin = new Thickness(0, 20, 0, 0);
                doc.Blocks.Add(total);
                Paragraph total2 = new Paragraph(new Run(SesionUsuario.Configuracion.MesnajeFinal2));//** NO ACEPTAMOS DEVOLUCION **
                total2.FontSize = 12;
                // total.FontWeight = FontWeights;
                total2.TextAlignment = TextAlignment.Center;
                total2.Margin = new Thickness(0, 0, 0, 0);
                doc.Blocks.Add(total2);


                //  VRelacionPorMes ventanaPrevia = new VRelacionPorMes(doc);
                // ventanaPrevia.ShowDialog();

                // Configurar e iniciar la impresión
                if (Prev)
                {
                    PrevDocumento documento = new PrevDocumento(doc);

                    var ret = documento.ShowDialog();
                    if (ret == true)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
                PrintDialog printDialog = new PrintDialog();
                if (true)
                {
                    IDocumentPaginatorSource idpSource = doc;

                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Factura #" + factura.Id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                //  MessageBox.Show("Error al imprimir: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }


        public async Task<List<Factura>> ObtenerFacturas()
        {
            return await db.Facturas.Where(x => x.Anulada == false).Include(x => x.Detalles).Include(x => x.Turno).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Factura?> ObtenerFacturaPorId(Factura factura)
        {
            try
            {
                factura = await db.Facturas?
                    .Include(f => f.Detalles)
                    .Where(f => f == factura)
                    .FirstOrDefaultAsync();
                if (factura == null) { factura = new Factura(); }
                return factura;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public async Task<bool> ActualizarFactura(Factura factura)
        {
            await db.Database.BeginTransactionAsync();
            try
            {

                if (!factura.Abierta)
                {
                    if (factura.Efectivo > factura.Total)
                    {
                        factura.Efectivo = factura.Total;
                    }
                    foreach (var item in factura.Detalles)
                    {
                        var p = await db.Productos.Include(x => x.Existencias).FirstOrDefaultAsync(x => x.Nombre == item.NombreProducto);

                        if (!p.EsServicio)
                        {

                            var exactual = p.Existencias.Sum(x => x.Entrada) - p.Existencias.Sum(x => x.Salida);
                            if (!SesionUsuario.Configuracion.VenderSinExistencia)
                            {
                                if (!p.PermitirStockNegativo)
                                {

                                    if (exactual < item.Cantidad)
                                    {
                                        MessageBox.Show("No hay suficiente en inventario del producto: " + p.Nombre, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                        await db.Database.RollbackTransactionAsync();
                                        return false;
                                    }
                                }
                            }

                            p.Existencias.Add(new Existencia
                            {
                                CantidadTotal = exactual - item.Cantidad,
                                FechaRegistro = DateTime.Now,
                                Descripcion = "Venta factura N° " + factura.Id,
                                Documento = "Factura N° " + factura.Id,
                                Entrada = 0,
                                Salida = item.Cantidad,
                                CostoUnitario = item.CostoUnitario,
                                PrecioUnitario = item.PrecioUnitario,
                                NombreUsuario = SesionUsuario.Usuario.NombreCompleto

                            });
                            if (p.Vence)
                            {
                                var lotes = await db.Lotes.Where(x => x.Cantidad > 0 && x.Producto.Id == p.Id).ToListAsync();
                                if (lotes.Count() > 0)
                                {
                                    Lote ellote;
                                    if (lotes.Count() > 1)
                                    {
                                        VLoteVenta vLoteVenta = new VLoteVenta(lotes);
                                        vLoteVenta.ShowDialog();
                                        var nl = vLoteVenta.codlote;
                                        ellote = lotes.FirstOrDefault(x => x.CodigoLote == nl);
                                        if (ellote.FechaVencimiento > lotes.OrderByDescending(x => x.FechaVencimiento).Last().FechaVencimiento)
                                        {
                                            if (MessageBox.Show("Del producto " + p.Nombre + " existe una fecha de vencimiento menor a la del lote indicado. Desea continuar de todos modos?", "Atención", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                                            {
                                                await db.Database.RollbackTransactionAsync();
                                                return false;
                                            }
                                        }
                                        ellote.Cantidad -= item.Cantidad;
                                        if (ellote.Cantidad > item.Cantidad && lotes.Count() > 2)
                                        {

                                        }
                                    }
                                    else
                                    {
                                        ellote = lotes.FirstOrDefault();
                                        if (ellote != null)
                                        {
                                            ellote.Cantidad -= item.Cantidad;
                                        }
                                    }
                                }

                            }
                        }

                        item.Producto = p;
                    }
                    await db.Existencias.AddRangeAsync(factura.Detalles.Select(d => d.Producto.Existencias.Last()));

                }

                // Obtener la factura existente con detalles
                var existingFactura = await db.Facturas
                    .Include(f => f.Detalles)
                    .FirstOrDefaultAsync(f => f.Id == factura.Id);

                if (existingFactura == null)
                {
                    return false; // La factura no existe
                }
                var toRemove = existingFactura.Detalles
                   .Where(rem => !factura.Detalles.Any(d => d.Id == rem.Id))
                   .ToList();

                foreach (var rem in toRemove)
                {
                    db.Entry(rem).State = EntityState.Deleted;
                    existingFactura.Detalles.Remove(rem);
                }
                // Actualizar propiedades escalares de la factura
                db.Entry(existingFactura).CurrentValues.SetValues(factura);

                var detallesexistentes = existingFactura.Detalles;



                // Actualizar los existentes y agregar los nuevos
                foreach (var item in factura.Detalles)
                {
                    var detalleExistente = existingFactura.Detalles.FirstOrDefault(ed => ed.Id == item.Id);
                    if (detalleExistente != null)
                    {
                        // Actualizar valores del detalle existente
                        db.Entry(detalleExistente).CurrentValues.SetValues(item);
                    }
                    else
                    {
                        // Nuevo detalle: agregar a la colección de la factura trackeada
                        existingFactura.Detalles.Add(item);
                    }
                }
                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await db.Database.RollbackTransactionAsync();
                MessageBox.Show("La actualización a fallado. " + ex.Message);
                throw;
            }
        }

        public bool SinNCF { get; set; } = true;
        private void addlinea(TableRow row, string text)
        {
            var paragraph = new Paragraph();
            paragraph.Margin = new Thickness(1);
            var cell = new TableCell(paragraph);
            cell.BorderThickness = new Thickness(0, 10, 0, 10);
            cell.FontSize = 0.5;
            cell.FontFamily = new FontFamily("Helvetica");
            cell.Background = Brushes.DarkBlue;
            row.Cells.Add(cell);
        }
        private void addImagen(TableRow row, string path)
        {
            var paragraph = new Paragraph();
            var cell = new TableCell(paragraph);


            if (!string.IsNullOrWhiteSpace(path))
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(path));
                paragraph.Inlines.Add(img);

            }
            cell.BorderThickness = new Thickness(0, 0, 0, 0);
            cell.FontSize = 0.5;
            cell.RowSpan = 5;
            cell.FontFamily = new FontFamily("Helvetica");
            //cell.Background = Brushes.Black;
            row.Cells.Add(cell);

        }
        private void addlinea2(TableRow row, string text)
        {
            var paragraph = new Paragraph();

            paragraph.Margin = new Thickness(10, 0, 10, 0);
            paragraph.LineHeight = 0.5;

            var cell = new TableCell(paragraph);
            cell.BorderThickness = new Thickness(0, 10, 0, 10);
            cell.FontSize = 0.5;

            cell.FontFamily = new FontFamily("Helvetica");
            cell.Background = Brushes.Black;
            row.Cells.Add(cell);
        }
        private void AddCell5(TableRow row, string text, TextAlignment alignment)
        {
            var paragraph = new Paragraph(new Run(text));
            paragraph.TextAlignment = alignment;
            paragraph.Margin = new Thickness(3);
            var cell = new TableCell(paragraph);
            cell.BorderBrush = Brushes.Black;
            cell.BorderThickness = new Thickness(0.5);
            cell.Padding = new Thickness(0.10);
            cell.FontFamily = new FontFamily("Helvetica");
            cell.FontSize = 12;
            row.Cells.Add(cell);
        }
        private void AddCell2(TableRow row, string text, TextAlignment alignment, FontWeight wei)
        {
            var paragraph = new Paragraph(new Run(text));
            paragraph.TextAlignment = alignment;

            paragraph.Margin = new Thickness(3);
            var cell = new TableCell(paragraph);
            cell.BorderBrush = Brushes.Black;
            cell.BorderThickness = new Thickness(0.5);
            cell.Padding = new Thickness(0.10);
            cell.Background = Brushes.LightGray;
            cell.FontWeight = wei;
            cell.FontFamily = new FontFamily("Helvetica");
            cell.FontSize = 12;
            row.Cells.Add(cell);
        }
        private void AddCell2(TableRow row, string text, TextAlignment alignment)
        {
            var paragraph = new Paragraph(new Run(text));
            paragraph.TextAlignment = alignment;

            paragraph.Margin = new Thickness(3);
            var cell = new TableCell(paragraph);
            cell.BorderBrush = Brushes.Black;
            cell.BorderThickness = new Thickness(0.5);
            cell.Padding = new Thickness(0.10);
            cell.Background = Brushes.LightGray;

            cell.FontFamily = new FontFamily("Helvetica");
            cell.FontSize = 12;
            row.Cells.Add(cell);
        }
        private void AddCellTitulo(TableRow row, string text, TextAlignment alignment, FontWeight fontW)
        {
            var paragraph = new Paragraph(new Run(text));
            paragraph.TextAlignment = alignment;

            var cell = new TableCell(paragraph);
            cell.FontSize = 30;
            cell.FontWeight = fontW;
            cell.Padding = new Thickness(1);
            cell.FontFamily = new FontFamily("Helvetica");

            row.Cells.Add(cell);
        }
        private void AddCellEnca(TableRow row, string text, TextAlignment alignment, FontWeight fontW)
        {
            var paragraph = new Paragraph(new Run(text));
            paragraph.TextAlignment = alignment;

            var cell = new TableCell(paragraph);
            cell.FontSize = 12;
            cell.FontWeight = fontW;
            cell.Padding = new Thickness(3);
            cell.FontFamily = new FontFamily("Helvetica");

            row.Cells.Add(cell);
        }
        private void AddCellEnca(TableRow row, string text, TextAlignment alignment)
        {
            var paragraph = new Paragraph(new Run(text));
            paragraph.TextAlignment = alignment;
            paragraph.Margin = new Thickness(3);
            var cell = new TableCell(paragraph);
            cell.BorderBrush = Brushes.Black;
            cell.Padding = new Thickness(0.10);
            cell.FontFamily = new FontFamily("Helvetica");
            cell.FontSize = 12;

            row.Cells.Add(cell);
        }
        private void AddCellEncaMixto(TableRow tr, string textoNegrita, string textoNormal, TextAlignment alignment)
        {
            var cell = new TableCell();
            var paragraph = new Paragraph();

            // Parte en negrita
            paragraph.Inlines.Add(new Run(textoNegrita) { FontWeight = FontWeights.Bold });
            // Parte normal
            paragraph.Inlines.Add(new Run(textoNormal));
            cell.FontFamily = new FontFamily("Helvetica");
            cell.FontSize = 14;
            paragraph.TextAlignment = alignment;
            cell.Blocks.Add(paragraph);
            tr.Cells.Add(cell);
        }
        public async Task<bool> ImprimirFacturaGrande(Factura factura, bool Prev = false)
        {
            try
            {


                string condicion = "";
                if (factura.TotalPendiente == 0)
                {
                    condicion = "Contado";
                }
                else
                {
                    condicion = "30 Dias Credito";
                }
                // Crear el documento de impresión
                FlowDocument doc = new FlowDocument();
                doc.PageHeight = 1056; // Tamaño carta
                doc.PageWidth = 816;
                doc.PagePadding = new Thickness(50, 5, 50, 25);
                doc.ColumnWidth = 816;
                doc.TextAlignment = TextAlignment.Center;


                Table TableEncabezado = new Table();
                TableEncabezado.CellSpacing = 0;
                TableEncabezado.Padding = new Thickness(0);
                TableEncabezado.Margin = new Thickness(0);
                TableEncabezado.TextAlignment = TextAlignment.Center;

                // Crear tabla de detalles
                Table table = new Table();
                table.CellSpacing = 0;
                table.Padding = new Thickness(0);
                table.Margin = new Thickness(0);
                table.TextAlignment = TextAlignment.Center;

                // Definir columnas
                TableEncabezado.Columns.Add(new TableColumn() { Width = new GridLength(500) }); // Producto
                TableEncabezado.Columns.Add(new TableColumn() { Width = new GridLength() }); // Unidad
                var gr = new TableRowGroup();
                var tr = new TableRow();
                TableCell cellimg = new TableCell();

                // Crear la imagen
                Image img = new Image();
                img.Source = Util.LoadImage(SesionUsuario.Configuracion.LogoPrincipal??new byte[0]);
                img.Width = 100;
                img.Height = 100;

                // Agregar la imagen a la celda
                Paragraph p = new Paragraph();
                p.TextAlignment = TextAlignment.Left;
                p.Inlines.Add(img);
                cellimg.Blocks.Add(p);

                // Añadir la celda a la fila
                tr.Cells.Add(cellimg);
                // AddCellEncaMixto(tr, "LIMSOTEC", "", TextAlignment.Left);
                AddCellEncaMixto(tr, "", "", TextAlignment.Left);
                gr.Rows.Add(tr);
                tr = new TableRow();
                AddCellTitulo(tr, SesionUsuario.Configuracion.NombreComercial, TextAlignment.Left, FontWeights.ExtraBold);
                AddCellEncaMixto(tr, $"FACTURA No.:" + factura.Id + "\nFecha Emisión: " + factura.FechaEmision.ToString("dd/MM/yyyy").ToUpper(), "", TextAlignment.Left);
                gr.Rows.Add(tr);
                tr = new TableRow();
                AddCellEncaMixto(tr, $"", SesionUsuario.Configuracion.Direccion, TextAlignment.Left);
                if (!SinNCF)
                    AddCellEncaMixto(tr, "Factura de Credito Fiscal", "", TextAlignment.Left);//
                gr.Rows.Add(tr);
                tr = new TableRow();
                AddCellEncaMixto(tr, $"", SesionUsuario.Configuracion.Telefono+" |  E-mail: " + SesionUsuario.Configuracion.Email, TextAlignment.Left);
                if (!SinNCF)
                    AddCellEncaMixto(tr, "NCF: B0100000002", "", TextAlignment.Left);//
                gr.Rows.Add(tr);
                tr = new TableRow();
                AddCellEncaMixto(tr, "RNC: "+SesionUsuario.Configuracion.RNC, "", TextAlignment.Left);
                if (!SinNCF)
                    AddCellEncaMixto(tr, $"Válido hasta: 31/12/2026", "", TextAlignment.Left);//
                gr.Rows.Add(tr);
                tr = new TableRow();
                AddCellEncaMixto(tr, "", "", TextAlignment.Left);
                AddCellEncaMixto(tr, "Condición: " + condicion, "", TextAlignment.Left);

                gr.Rows.Add(tr);
                tr = new TableRow();
                addlinea(tr, "");
                addlinea(tr, "");

                gr.Rows.Add(tr);


                tr = new TableRow();
                AddCellEncaMixto(tr, "Cliente: ", factura.NombreCliente, TextAlignment.Left);
                AddCellEncaMixto(tr, "", "", TextAlignment.Left);
                gr.Rows.Add(tr);
                tr = new TableRow();
                AddCellEncaMixto(tr, "RNC Cliente: ", factura.RNCCliente ?? "", TextAlignment.Left);
                AddCellEncaMixto(tr, "E-Mail: ", "", TextAlignment.Left);
                gr.Rows.Add(tr);
                tr = new TableRow();
                AddCellEncaMixto(tr, "Dirección: ", factura.DireccionCliente ?? "San Juan", TextAlignment.Left);
                AddCellEncaMixto(tr, "Tel.:", factura.TelefonoCliente ?? "", TextAlignment.Left);
                gr.Rows.Add(tr);
                tr = new TableRow();
                addlinea(tr, "");
                addlinea(tr, "");

                gr.Rows.Add(tr);

                TableEncabezado.RowGroups.Add(gr);


                table.Columns.Add(new TableColumn() { Width = new GridLength(70) }); // Producto
                table.Columns.Add(new TableColumn() { Width = new GridLength() }); // Unidad
                table.Columns.Add(new TableColumn() { Width = new GridLength(70) }); // Cantidad
                table.Columns.Add(new TableColumn() { Width = new GridLength(70) }); // Cantidad
                table.Columns.Add(new TableColumn() { Width = new GridLength(70) }); // Cantidad
                table.Columns.Add(new TableColumn() { Width = new GridLength(70) }); // Cantidad
                table.Columns.Add(new TableColumn() { Width = new GridLength(100) }); // Cantidad

                //700

                // Crear encabezados
                TableRowGroup headerGroup = new TableRowGroup();
                TableRow headerRow = new TableRow();

                string[] headers = new[] { "Cantidad", "Descripcionón", "Unidad", "Precio", "ITBIS", "Descuento", "Valor" };
                foreach (var header in headers)
                {
                    var grp = new Paragraph(new Run(header));
                    grp.TextAlignment = TextAlignment.Center;
                    grp.Margin = new Thickness(0);
                    grp.LineHeight = 18; // altura fija para control visual
                    TableCell cell = new TableCell(grp);
                    cell.Background = Brushes.LightGray;
                    cell.FontWeight = FontWeights.Bold;
                    cell.BorderBrush = Brushes.Black;
                    cell.Blocks.FirstBlock.TextAlignment = TextAlignment.Center;
                    cell.RowSpan = 2; // usualmente necesario solo si la fila es alta
                    cell.Blocks.FirstBlock.Margin = new Thickness(1);
                    cell.Blocks.FirstBlock.LineHeight = 20;
                    cell.FontSize = 14;

                    cell.TextAlignment = TextAlignment.Center;
                    cell.BorderThickness = new Thickness(1, 1, 1, 1);
                    cell.FontFamily = new FontFamily("Calibri");
                    cell.Padding = new Thickness(1);
                    headerRow.Cells.Add(cell);
                }

                headerGroup.Rows.Add(headerRow);
                table.RowGroups.Add(headerGroup);
                // Añadir detalles
                TableRowGroup dataGroup = new TableRowGroup();
                decimal TotalItebis = 0;
                foreach (var detalle in factura.Detalles)
                {

                    TableRow row = new TableRow();
                    row.FontSize = 14;
                    AddCell5(row, detalle.Cantidad.ToString("N2"), TextAlignment.Center);
                    AddCell5(row, $"{detalle.NombreProducto}", TextAlignment.Left);
                    AddCell5(row, "UND", TextAlignment.Center);
                    AddCell5(row, (detalle.PrecioUnitario).ToString("N2"), TextAlignment.Right);
                    AddCell5(row, (detalle.ITBIS).ToString("N2"), TextAlignment.Right);
                    AddCell5(row, detalle.Descuento.ToString("N2"), TextAlignment.Right);
                    AddCell2(row, detalle.SubTotal.ToString("N2"), TextAlignment.Right);
                    TotalItebis = TotalItebis + (detalle.Cantidad * detalle.ITBIS);

                    dataGroup.Rows.Add(row);
                }

                table.RowGroups.Add(dataGroup);
                doc.Blocks.Add(TableEncabezado);
                doc.Blocks.Add(table);
                // Añadir línea horizontal
                Border lineaHorizontal = new Border();
                lineaHorizontal.BorderBrush = Brushes.Black;
                lineaHorizontal.BorderThickness = new Thickness(0, 1, 0, 0);
                lineaHorizontal.Margin = new Thickness(0, 10, 0, 10);
                lineaHorizontal.Width = 716; // Ancho total menos padding del documento

                Table Tablepie = new Table();
                table.FontSize = 12;
                Tablepie.CellSpacing = 0;
                Tablepie.Padding = new Thickness(0);
                // Establecer margen superior grande para empujar la tabla hacia abajo
                Tablepie.Margin = new Thickness(0, 0, 0, 0);
                Tablepie.TextAlignment = TextAlignment.Center;

                // Definir columnas
                Tablepie.Columns.Add(new TableColumn() { Width = new GridLength() });
                Tablepie.Columns.Add(new TableColumn() { Width = new GridLength() });
                Tablepie.Columns.Add(new TableColumn() { Width = new GridLength() });
                Tablepie.Columns.Add(new TableColumn() { Width = new GridLength() });
                Tablepie.Columns.Add(new TableColumn() { Width = new GridLength(125) });
                Tablepie.Columns.Add(new TableColumn() { Width = new GridLength(100) });

                var grP = new TableRowGroup();
                //var trP = new TableRow();
                //AddCellEnca(trP, "", TextAlignment.Left);
                //AddCellEnca(trP, "", TextAlignment.Left);
                //grP.Rows.Add(trP);

                var trP = new TableRow();

                addlinea(trP, "");
                addlinea(trP, "");
                addlinea(trP, "");
                addlinea(trP, "");
                addlinea(trP, "");
                addlinea(trP, "");
                grP.Rows.Add(trP);

                trP = new TableRow();
                addImagen(trP, "");
                AddCellEnca(trP, "", TextAlignment.Left);
                AddCellEnca(trP, "", TextAlignment.Left);
                AddCellEnca(trP, "", TextAlignment.Left);
                AddCellEnca(trP, "Subtotal Gravado:", TextAlignment.Right);
                AddCell2(trP, (factura.SubTotal).ToString("N2"), TextAlignment.Right);
                grP.Rows.Add(trP);
                trP = new TableRow();
                AddCellEnca(trP, "", TextAlignment.Left);
                AddCellEnca(trP, "", TextAlignment.Left);
                AddCellEnca(trP, "", TextAlignment.Left);
                AddCellEnca(trP, "Subtotal Exento:", TextAlignment.Right);
                AddCell2(trP, (0).ToString("N2"), TextAlignment.Right);
                grP.Rows.Add(trP);
                trP = new TableRow();
                AddCellEnca(trP, "", TextAlignment.Left);
                AddCellEnca(trP, "", TextAlignment.Left);
                AddCellEnca(trP, "", TextAlignment.Left);
                AddCellEnca(trP, "Total Descuento:", TextAlignment.Right);
                AddCell2(trP, factura.Detalles.Sum(x => x.Descuento).ToString("N2"), TextAlignment.Right);
                grP.Rows.Add(trP);
                trP = new TableRow();
                AddCellEnca(trP, "", TextAlignment.Left);
                AddCellEnca(trP, "", TextAlignment.Left);
                addlinea2(trP, "");
                AddCellEnca(trP, "Total ITBIS:", TextAlignment.Right);
                AddCell2(trP, factura.ITBIS.ToString("N2"), TextAlignment.Right);
                grP.Rows.Add(trP);
                trP = new TableRow();
                AddCellEnca(trP, "", TextAlignment.Center);
                AddCellEnca(trP, "", TextAlignment.Left);
                AddCellEnca(trP, "Recibido Por", TextAlignment.Center);
                AddCellEnca(trP, "Total:", TextAlignment.Right, FontWeights.Bold);
                AddCell2(trP, factura.Total.ToString("N2"), TextAlignment.Right, FontWeights.Bold);
                grP.Rows.Add(trP);
                if (factura.TotalPendiente > 0)
                {
                    trP = new TableRow();
                    AddCellEnca(trP, "", TextAlignment.Right);
                    AddCellEnca(trP, "", TextAlignment.Right);
                    AddCellEnca(trP, "", TextAlignment.Right);
                    AddCellEnca(trP, "", TextAlignment.Right);
                    AddCellEnca(trP, "Total Pendiente:", TextAlignment.Right);
                    AddCell2(trP, factura.TotalPendiente.ToString("N2"), TextAlignment.Right);
                    grP.Rows.Add(trP);
                }


                Tablepie.RowGroups.Add(grP);

                // Crear contenedor para forzar posición al final

                // pieContainer.Child = Tablepie;
                doc.Blocks.Add(Tablepie);
                var final = new Paragraph(new Run(@$"{SesionUsuario.Configuracion.MesnajeFinal1}
{SesionUsuario.Configuracion.MesnajeFinal2}" ));
                final.TextAlignment = TextAlignment.Justify;
                final.FontSize = 10;
                // 🔹 Línea azul 3 cm antes del final de la página
                // 1 cm ≈ 37.8 px → 3 cm ≈ 113 px
                BlockUIContainer lineaFinal = new BlockUIContainer();
                Border lineaAzul = new Border
                {
                    BorderBrush = Brushes.DarkBlue,
                    BorderThickness = new Thickness(0, 1, 0, 0),
                    Width = double.NaN, // se ajusta al ancho disponible
                    Margin = new Thickness(0, 0, 0, 113) // 3 cm desde abajo
                };
                lineaFinal.Child = lineaAzul;

                // Agregar al documento
                doc.Blocks.Add(final);
                doc.Blocks.Add(lineaFinal);

                // Mostrar vista previa
                //   VRelacionPorMes ventanaPrevia = new VRelacionPorMes(doc);
                //    ventanaPrevia.ShowDialog();

                // Imprimir
                if (Prev)
                {
                    PrevDocumento documento = new PrevDocumento(doc);
                  
                var ret  =  documento.ShowDialog();
                    if (ret==true)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
                PrintDialog printDialog = new PrintDialog();
                if (true)//printDialog.ShowDialog() == true
                {
                    IDocumentPaginatorSource idpSource = doc;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, $"Factura N° {factura.Numero}");
                    return true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir el conduce. Por favor, intente nuevamente. Mensaje: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<bool> AnularFactura(Factura factura)
        {
            if (!SesionUsuario.Usuario.AnularFacturas)
            {
                MessageBox.Show("El usuario no tiene permisos para anular facturas");
            }
            if (factura == null)
            {
                return false;
            }
            if (factura.Anulada)
            {
                return true;
            }
            if (factura.Turno != null && factura.Turno.Abierto == false)
            {
                MessageBox.Show("No se puede anular una factura de un turno cerrado.");
                return false;
            }
            factura.Anulada = true;
            factura.Condicion= "NULA";
            factura.Efectivo = 0;
            factura.TotalPagado = 0;
            factura.TotalPendiente = 0;
            factura.DiasCredito = 0;
            factura.Tarjeta = 0;
            factura.Propina = 0;
            factura.Cheque = 0;
            factura.Abierta = false;
            db.Database.BeginTransaction();
            //  List<Existencia> existencias = new List<Existencia>();
            try
            {
                if (!factura.Abierta)
                {
                    foreach (var item in factura.Detalles)
                    {
                        var p = await db.Productos.FirstOrDefaultAsync(x => x.Nombre == item.NombreProducto);
                        if (!p.EsServicio)
                        {
                            p.Existencias.Add(new Existencia
                            {
                                CantidadTotal = item.Cantidad,
                                FechaRegistro = DateTime.Now,
                                Descripcion = "Anulación de factura N° " + factura.Id,
                                Documento = "Factura N° " + factura.Id,
                                Entrada = item.Cantidad,
                                Salida = 0,
                                CostoUnitario = item.CostoUnitario,
                                PrecioUnitario = item.PrecioUnitario,
                                NombreUsuario = SesionUsuario.Usuario.NombreCompleto
                            });
                            //  existencias.Add(p.Existencias.Last());


                        }
                        item.Producto = p;
                    }
                }

                db.Facturas.Update(factura);
                await db.Existencias.AddRangeAsync(factura.Detalles.Select(d => d.Producto.Existencias.Last()));

                // await  db.Existencias.AddRangeAsync(existencias);
                await db.SaveChangesAsync();
                db.Database.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                db.Database.RollbackTransaction();
                MessageBox.Show("La anulación a fallado. " + ex.Message);
                throw;

            }
        }
    }
}
