using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SFCH.Controller;
using SFCH.IService;
using SFCH.Model;
using SFCH.PrintView;
using SFCH.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SFCH.Logica
{
    public class TurnoService : ITurno
    {
        Conexion db = new Conexion();
        public async Task<bool> CerrarTurnoAsync(Turno turnot)
        {
            await db.Database.BeginTransactionAsync();
            try
            {
               
                var turno=db.Turnos.Include(x => x.DesgloseBilletes).Include(x => x.ContratosTractor).Include(x => x.DetalleTurnos).Include(x => x.Usuario).FirstOrDefault(t => t.Id == turnot.Id);

                if (turno == null || !turno.Abierto)
                {
                    MessageBox.Show("No hay un turno abierto para este usuario.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                var facturasAbiertas = await db.Facturas
                    .Where(f => f.Turno.Id == turno.Id && f.Abierta && !f.Anulada)
                    .ToListAsync();

                if (facturasAbiertas.Count > 0)
                {
                    MessageBox.Show("No se puede cerrar el turno porque hay facturas abiertas asociadas.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                // Copiar valores escalares
                db.Entry(turno).CurrentValues.SetValues(turnot);

                // Manejar DesgloseBilletes: agregar solo los nuevos
                foreach (var item in turnot.DesgloseBilletes)
                {
                    if (item.Id == 0)
                    {
                        item.Turno = turno; // asegurar la relación
                        await db.DesglosesBilletes.AddAsync(item);
                    }
                    else
                    {
                        // si pueden existir items editables, actualizar su estado/valores
                        db.DesglosesBilletes.Update(item);
                    }
                }
                
             
                turno.FechaFin = DateTime.Now;
                turno.Abierto = false;
                turno.Estado = "Cerrado";
                
                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
                await imprimirCuadreAsync(turno);
                return true;
            }
            catch
            {
                await db.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> IniciarTurnoAsync(decimal MontoInicial)
        {
            await db.Database.BeginTransactionAsync();
            try
            {
                var turnoExistente = await db.Turnos
                    .Where(t => t.Usuario == SesionUsuario.Usuario && t.Abierto)
                    .FirstOrDefaultAsync();
                if (turnoExistente != null)
                {
                    MessageBox.Show("Ya existe un turno abierto para este usuario.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    // Ya hay un turno abierto para este usuario
                    return false;
                }
                var nuevoTurno = new Turno
                {
                    Usuario = await db.Usuarios.FindAsync(SesionUsuario.Usuario.Id),
                    FechaInicio = DateTime.Now,
                    TotalInicial = MontoInicial, // Puedes ajustar esto según tus necesidades
                    TotalFinal = 0m
                    ,
                    Abierto = true,
                    Estado = "Abierto",
                    NombreUsuario = SesionUsuario.Usuario.NombreUsuario
                };
                await db.Turnos.AddAsync(nuevoTurno);
                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
                return true;

            }
            catch (Exception ex)
            {
                await db.Database.RollbackTransactionAsync();
                //  return false;
                throw;
            }
        }

        public Task<decimal> ObtenerSaldoCajaAsync(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Turno>> ObtenerTurnosAsync()
        {
            return await db.Turnos.Include(x => x.Facturas).Include(x=>x.DesgloseBilletes).Include(x=>x.ContratosTractor).Include(x => x.DetalleTurnos).Include(x => x.Usuario).OrderDescending().AsNoTracking().ToListAsync();
        }

        public Task<bool> RegistrarMovimientoCajaAsync(Factura factra)
        {
            throw new NotImplementedException();
        }
        public async Task<int> FacturasAbiertas(Turno turno)
        {
            var count = await db.Facturas
                .Where(f => f.Anulada == false && f.Turno == turno && f.Abierta)
                .CountAsync();
            return count;

        }

        public async Task<bool> VerificarTurnoAbiertoAsync(Usuario usuarioId)
        {
            var turnoAbierto = await db.Turnos
                .Where(t => t.Usuario == usuarioId && t.Abierto)
                .FirstOrDefaultAsync();

            if (turnoAbierto?.Abierto ?? false)
            {
                SesionUsuario.TurnoActual = turnoAbierto;
                return true;
            }
            return false;
        }

        public async Task<List<Factura>> OptenerFacturasAbiertas(Turno turno)
        {
            try
            {
                var count = await db.Facturas
                .Where(f => f.Anulada == false && f.Turno == turno && f.Abierta).Include(x => x.Detalles).ToListAsync();

                return count;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> imprimirCuadreAsync(Turno turno)
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
                RNC.FontWeight = FontWeights.Bold;
                RNC.Margin = new Thickness(0, 0, 0, 1);

                Paragraph title = new Paragraph(new Run("Cuadre de Caja Turno #: " + turno.Id));
                title.FontSize = 12;
                title.TextAlignment = TextAlignment.Center;
                title.FontWeight = FontWeights.Bold;
                title.Margin = new Thickness(0, 0, 0, 2);

                Paragraph fecha = new Paragraph(new Run("Inicio: " + turno.FechaInicio.ToString("dd/MM/yyyy hh:mm:tt")));
                fecha.FontSize = 12;
                fecha.FontFamily = new FontFamily("Calibri");
                fecha.TextAlignment = TextAlignment.Right;
                fecha.Margin = new Thickness(0, 0, 0, 0);

                Paragraph fecha2 = new Paragraph(new Run("Final: " + turno.FechaFin.ToString("dd/MM/yyyy hh:mm:tt")));
                fecha2.FontSize = 12;
                fecha2.FontFamily = new FontFamily("Calibri");
                fecha2.TextAlignment = TextAlignment.Right;
                fecha2.Margin = new Thickness(0, 0, 0, 0);

                Paragraph condi = new Paragraph(new Run("Usuario: " + turno.NombreUsuario));
                condi.FontSize = 12;
                condi.FontFamily = new FontFamily("Calibri");
                condi.TextAlignment = TextAlignment.Left;
                condi.Margin = new Thickness(0, 0, 0, 2);

                Paragraph subtitle = new Paragraph(new Run("Estado: " + turno.Estado));
                subtitle.FontSize = 12;
                subtitle.FontFamily = new FontFamily("Calibri");
                subtitle.TextAlignment = TextAlignment.Left;
                subtitle.Margin = new Thickness(0, 0, 0, 0);
                Paragraph TelCliente = new Paragraph(new Run("Tel.:" + turno.Efectivo));
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
                doc.Blocks.Add(fecha2);
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
                int totalWidth = 315;
                table.Columns.Add(new TableColumn() { Width = new GridLength() });
                // table.Columns.Add(new TableColumn() { Width = new GridLength(50) });
                table.Columns.Add(new TableColumn() { Width = new GridLength(50) });
                table.Columns.Add(new TableColumn() { Width = new GridLength(50) });
                table.Columns.Add(new TableColumn() { Width = new GridLength(50) });



                // Crear encabezados
                TableRowGroup headerGroup = new TableRowGroup();
                TableRow headerRow = new TableRow();
                string[] heders = { "Documento", "Crédito", "Pagado", "Total" };
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
                foreach (var item in turno.Facturas)
                {


                    TableRow row2 = new TableRow();

                    // Crear una celda que abarque todas las columnas para mostrar el nombre del producto centrado
                    var paragraph = new Paragraph(new Run("FAC #" + item.Id + " - " + item.Condicion.ToUpperInvariant() + " - " + "EFECTIVO")) { TextAlignment = TextAlignment.Left };
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
                    // AddCellNul(row, item.SubTotal.ToString("N"), TextAlignment.Center);
                    AddCellNul(row, item.TotalPendiente.ToString("N"), TextAlignment.Center);
                    AddCellNul(row, item.Efectivo.ToString("N"), TextAlignment.Center);
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
                tablet.Columns.Add(new TableColumn() { Width = new GridLength(150) });
                tablet.Columns.Add(new TableColumn() { Width = new GridLength() });

                TableRowGroup dataGroupT = new TableRowGroup();

                var x1t = new TableRow();
                AddCellNul3(x1t, "", TextAlignment.Right);
                AddCellNul3(x1t, "EFECTIVO INICIAL:", TextAlignment.Right);
                AddCellNul3(x1t, turno.TotalInicial.ToString("C"), TextAlignment.Right);
                dataGroupT.Rows.Add(x1t);

                var xtv = new TableRow();
                AddCellNul(xtv, "", TextAlignment.Right);
                AddCellNul(xtv, "VENTA TOTAL:", TextAlignment.Right);
                AddCellNul(xtv, turno.Ventas.ToString("C"), TextAlignment.Right);
                dataGroupT.Rows.Add(xtv);

                if (turno.Facturas.Sum(x => x.TotalPendiente) > 0)
                {
                    var xt = new TableRow();
                    AddCellNul(xt, "", TextAlignment.Right);
                    AddCellNul(xt, "VENTA CREDITO:", TextAlignment.Right);
                    AddCellNul(xt, turno.Facturas.Sum(x => x.TotalPendiente).ToString("C"), TextAlignment.Right);
                    dataGroupT.Rows.Add(xt);
                }
                if (turno.EfectivoContado > 0)
                {
                    var xte = new TableRow();
                    AddCellNul(xte, "", TextAlignment.Right);
                    AddCellNul(xte, "VENTA EFECTIVO:", TextAlignment.Right);
                    AddCellNul(xte, turno.EfectivoContado.ToString("C"), TextAlignment.Right);
                    dataGroupT.Rows.Add(xte);
                }
                if (turno.Tarjetas > 0)
                {
                    var x2tp = new TableRow();
                    AddCellNul(x2tp, "", TextAlignment.Right);
                    AddCellNul(x2tp, "TARJETA:", TextAlignment.Right);
                    AddCellNul(x2tp, turno.Tarjetas.ToString("C"), TextAlignment.Right);
                    dataGroupT.Rows.Add(x2tp);
                }
                if (turno.Cheques>0)
                {
                    var x2tpC = new TableRow();
                    AddCellNul(x2tpC, "", TextAlignment.Right);
                    AddCellNul(x2tpC, "Cheques:", TextAlignment.Right);
                    AddCellNul(x2tpC, turno.Cheques.ToString("C"), TextAlignment.Right);
                    dataGroupT.Rows.Add(x2tpC);
                }
                if (turno.ContratosTractor.Sum(x=>x.TotalPagado) > 0)
                {
                    var x2tpC = new TableRow();
                    AddCellNul(x2tpC, "", TextAlignment.Right);
                    AddCellNul(x2tpC, "Contrato Efectivo:", TextAlignment.Right);
                    AddCellNul(x2tpC, turno.ContratosTractor.Sum(x => x.TotalPagado).ToString("C"), TextAlignment.Right);
                    dataGroupT.Rows.Add(x2tpC);
                }
                var x2t = new TableRow();
                AddCellNul3(x2t, "", TextAlignment.Right);
                AddCellNul3(x2t, "EFECTIVO FINAL:", TextAlignment.Right);
                AddCellNul3(x2t, turno.TotalFinal.ToString("C"), TextAlignment.Right);
                dataGroupT.Rows.Add(x2t);

                var x4t = new TableRow();
                AddCellNul(x4t, "", TextAlignment.Right);
                AddCellNul(x4t, "EFECTIVO CONTADO:", TextAlignment.Right, FontWeights.Bold);
                AddCellNul(x4t, turno.Efectivo.ToString("C"), TextAlignment.Right, FontWeights.Bold);
                dataGroupT.Rows.Add(x4t);
                
                
                var x2tpCP = new TableRow();
                AddCellNul(x2tpCP, "", TextAlignment.Right);
                AddCellNul(x2tpCP, "DIFERENCIA:", TextAlignment.Right, FontWeights.ExtraBold);
                AddCellNul(x2tpCP, turno.Diferencia.ToString("C"), TextAlignment.Right, FontWeights.ExtraBold);
                dataGroupT.Rows.Add(x2tpCP);
                tablet.RowGroups.Add(dataGroupT);

                tablet.TextAlignment = TextAlignment.Center;

                doc.Blocks.Add(tablet);
                Paragraph Nota = new Paragraph(new Run("Observación: "+turno.Observaciones));
                Nota.FontSize = 14;
                Nota.FontFamily = new FontFamily("Calibri");
                Nota.FontWeight = FontWeights.ExtraBlack;
                Nota.TextAlignment = TextAlignment.Left;
                Nota.Margin = new Thickness(10, 0, 0, 0);

                doc.Blocks.Add(Nota);
                if (turno.DesgloseBilletes.Count() > 0)
                {
                    Paragraph TituloDesglose = new Paragraph(new Run("Desglose de Billetes: "));
                    TituloDesglose.FontSize = 12;
                    TituloDesglose.FontFamily = new FontFamily("Calibri");
                    TituloDesglose.TextAlignment = TextAlignment.Left;
                    TituloDesglose.Margin = new Thickness(10, 0, 0, 0);

                    doc.Blocks.Add(TituloDesglose);
                }
                foreach (var item in turno.DesgloseBilletes.OrderByDescending(x=>x.Denominacion))
                {
                    Paragraph desglose = new Paragraph(new Run(item.Texto));
                    desglose.FontSize = 14;
                    desglose.FontFamily = new FontFamily("Calibri");
                    desglose.TextAlignment = TextAlignment.Left;
                    desglose.Margin = new Thickness(10, 0, 0, 0);

                    doc.Blocks.Add(desglose);
                }

                Paragraph TotalB = new Paragraph(new Run("Total : "+turno.DesgloseBilletes.Sum(x=>x.Total).ToString("C")));
                TotalB.FontSize = 14;
                TotalB.FontFamily = new FontFamily("Calibri");
                TotalB.FontWeight = FontWeights.Bold;
                TotalB.TextAlignment = TextAlignment.Left;
                TotalB.Margin = new Thickness(10, 0, 0, 0);

                doc.Blocks.Add(TotalB);
                Paragraph vendedor = new Paragraph(new Run("Montos en RD$ (Pesos Dominicanos)"));
                vendedor.FontSize = 12;
                vendedor.FontFamily = new FontFamily("Calibri");
                vendedor.TextAlignment = TextAlignment.Left;
                vendedor.Margin = new Thickness(0, 0, 0, 0);

                doc.Blocks.Add(vendedor);


                // Añadir total al final
                Paragraph total = new Paragraph(new Run("!! Gracias Por Su Compra Y Feliz Navidad !!"));
                total.FontSize = 12;
                // total.FontWeight = FontWeights;
                total.TextAlignment = TextAlignment.Center;
                total.Margin = new Thickness(10, 20, 0, 0);
                // doc.Blocks.Add(total);
                Paragraph total2 = new Paragraph(new Run("** NO MAS LINEAS, FIN DEL CUADRE **"));
                total2.FontSize = 12;
                // total.FontWeight = FontWeights;
                total2.TextAlignment = TextAlignment.Center;
                total2.Margin = new Thickness(0, 0, 0, 0);
                doc.Blocks.Add(total2);


                //  VRelacionPorMes ventanaPrevia = new VRelacionPorMes(doc);
                // ventanaPrevia.ShowDialog();

                // Configurar e iniciar la impresión
                PrintDialog printDialog = new PrintDialog();
                PrevDocumento prevDocumento = new PrevDocumento(doc);
                if (prevDocumento.ShowDialog() == false)
                {
                    return false;
                }
                if (true)
                {
                    IDocumentPaginatorSource idpSource = doc;

                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Cuadre de caja turno #" + turno.Id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir cuadre : " + ex.Message);
                //  MessageBox.Show("Error al imprimir: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        private void AddCellNul(TableRow row, string text, TextAlignment alignment = TextAlignment.Left, FontWeight? fontWeight = null)
        {
            // Usar el FontWeight proporcionado o el valor por defecto en tiempo de ejecución.
            var usedFontWeight = fontWeight ?? FontWeights.Normal;

            var paragraph = new Paragraph(new Run(text)) { TextAlignment = alignment };
            TableCell cell = new TableCell(paragraph);
            cell.FontSize = 12;
            cell.FontWeight = usedFontWeight;
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

        public async Task<DetalleTurno> RegistrarEgreso(string Nombre, decimal Monto)
        {
            try
            {
                var re = new DetalleTurno
                {
                    Egreso = Monto,
                    Ingreso = 0,
                    Descripcion = Nombre,
                    Turno = await db.Turnos.FindAsync(SesionUsuario.TurnoActual.Id)
                };
                db.DetalleTurnos.Add(re);
                await db.SaveChangesAsync();
                return re;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el egreso: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new DetalleTurno();
            }
        }

        public async Task<Turno> ObtenerTurnoAsync(int id)
        {
            return await db.Turnos.Include(x => x.DesgloseBilletes).Include(x=>x.Facturas).Include(x => x.ContratosTractor).Include(x => x.DetalleTurnos).Include(x => x.Usuario).FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
