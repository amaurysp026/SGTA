using Microsoft.EntityFrameworkCore;
using SFCH.Controller;
using SFCH.IService;
using SFCH.Model;
using SFCH.PrintView;
using SFCH.View;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using MessageBox = System.Windows.Forms.MessageBox;
using PrintDialog = System.Windows.Controls.PrintDialog;

namespace SFCH.Logica
{
    public class CuentaService : ICuenta
    {
        public async Task<bool> AnularTransa(Transaccion transaccion)
        {
            try
            {
                using (var db = new Conexion())
                {
                    var t = await db.Database.BeginTransactionAsync();

                    var tras = await db.Transacciones.FirstOrDefaultAsync(x => x.Id == transaccion.Id);
                    tras.Nula = true;
                    if (await db.SaveChangesAsync() > 0)
                    {
                        await t.CommitAsync();
                        await CalcularTotal(transaccion.Cuenta);
                        return true;
                    }
                    else
                    {
                        await t.RollbackAsync();
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                return false;

            }
        }

        public async Task<bool> CalcularTotal(Cuenta cuenta)
        {
            using (var db = new Conexion())
            {
                var t = await db.Database.BeginTransactionAsync();
                try
                {
                    var c = await db.Cuentas.Include(x=>x.Transacciones).FirstOrDefaultAsync(x => x.Id == cuenta.Id);
                    var sal= c.Transacciones.Where(x => x.Nula == false).Sum(x => x.MontoCredito) - c.Transacciones.Where(x => x.Nula == false).Sum(x => x.MontoDebito);
                //  MessageBox.Show("Saldo calculado: "+sal);
                    c.Saldo = sal;

                    if (await db.SaveChangesAsync() > 0)
                    {
                        await t.CommitAsync();
                        return true;
                    }
                    else
                    {
                        await t.RollbackAsync();
                        return false;
                    }
                }
                catch (Exception)
                {
                    await t.RollbackAsync();
                    return false;
                }
            }
        }
        public async Task<bool> CalcularTodoTotal()
        {
            using (var db = new Conexion())
            {
                var t = await db.Database.BeginTransactionAsync();
                try
                {
                  
                    var c = db.Cuentas.Include(x => x.Transacciones);
                   
                 //   MessageBox.Show("Cuentas a recalcular: "+c.Count());
                    foreach (var item in c)
                    {
                        item.Saldo = item.Transacciones.Where(x => x.Nula == false).Sum(x => x.MontoCredito) - item.Transacciones.Where(x => x.Nula == false).Sum(x => x.MontoDebito);
                       
                    }
                    if (await db.SaveChangesAsync() > 0)
                    {
                        await t.CommitAsync();
                        MessageBox.Show("Recalculo con exito");
                        return true;
                    }
                    else
                    {
                        await t.RollbackAsync();
                        MessageBox.Show("Ningun elemento recalculado ");

                        return false;
                    }
                    
                }
                catch (Exception)
                {
                    await t.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> DesactivarCuenta(Cuenta cuenta)
        {
            using (var db=new Conexion())
            {
             await   db.Database.BeginTransactionAsync();
                try
                {
                    var c = await db.Cuentas.FirstOrDefaultAsync(x => x.Id == cuenta.Id);
                    if (c == null)
                    {
                        MessageBox.Show("Aviso!", "La cuenta no existe en base de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    c.Activo = false;
                    if (await db.SaveChangesAsync() > 0)
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
                    MessageBox.Show("Error al desactivar la cuenta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        public Task<bool> EliminarCuenta(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GuardarCuenta(Cuenta cuenta)
        {

            try
            {
                using (var db = new Conexion())
                {
                    cuenta.Titular = await db.Personas.FindAsync(cuenta.Titular.Id);
                    cuenta.TipoCuenta = await db.TipoCuentas.FindAsync(cuenta.TipoCuenta.Id);
                    if (cuenta.TipoCuenta == null)
                    {
                        MessageBox.Show("Aviso!", "El tipo de cuenta no existen en base de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    if (cuenta.Titular == null)
                    {
                        MessageBox.Show("Aviso!", "El titular no existen en base de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    cuenta.Moneda = SesionUsuario.Configuracion.Moneda ?? "DOP";
                    Cuenta e = await db.Cuentas.AsNoTracking().OrderBy(x => x.Id).LastOrDefaultAsync() ?? new Cuenta();
                    string e2 = (e?.Id + 1 ?? 1).ToString();
                    var t = cuenta.TipoCuenta.Id.ToString();
                    string personaId = cuenta.Titular.Id.ToString();
                    cuenta.NumeroCuenta = t.PadRight(10 - t.Length - e2.Length - personaId.Length, '0') + personaId + e2
                                          ;
                    await db.Cuentas.AddAsync(cuenta);
                    return await db.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la cuenta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        public Task<Cuenta> ObtenerCuentaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Cuenta>> ObtenerCuentas()
        {
            try
            {
                using (var db = new Conexion())
                {
                    return await db.Cuentas.Where(x => x.Activo).Include(x => x.Titular).ThenInclude(x => x.TipoEntidad).Include(x => x.TipoCuenta).Include(x => x.Transacciones.Where(x => x.Nula == false)).AsNoTracking().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar cuentas");
                throw;
            }
        }

        public async Task<bool> TrasnIngreso(Transaccion transaccion)
        {
            using (var db = new Conexion())
            {
                // MessageBox.Show("Iniciando transacción");
                var t = await db.Database.BeginTransactionAsync();
                try
                {
               
                    Cuenta cuet = await db.Cuentas.Include(x=>x.Titular).Include(x => x.Transacciones).FirstOrDefaultAsync(x => x == transaccion.Cuenta);
                    if (cuet == null)
                    {
                        MessageBox.Show("Cuenta no encontrada", "La cuenta digitada no es valida o no se cuentra en la base de datos", MessageBoxButtons.OK, MessageBoxIcon.Question); return false;

                    }
                    cuet.Transacciones.Add(transaccion);

                    if (db.SaveChanges() > 0)
                    {
                        cuet.Saldo = cuet.Transacciones.Where(x => x.Nula == false).Sum(x => x.MontoCredito) - cuet.Transacciones.Sum(x => x.MontoDebito);
                        if (db.SaveChanges() > 0)
                        {
                            PPuntoVen pPuntoVen = new PPuntoVen(cuet.Transacciones.LastOrDefault());
                           await t.CommitAsync();
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Error", "No fue posible actualizar el saldo de la cuenta");
                            await t.RollbackAsync();

                            return false;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Error", "No fue posible registrar la transacción");
                        await t.RollbackAsync();

                        return false;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Inesperado", "Un error bloquea la transacción :" + ex.Message);
                 await   t.RollbackAsync();
                    return false;
                }
            }


        }
        private async void AddCell(TableRow row, string text, TextAlignment alignment = TextAlignment.Left)
        {
            var paragraph = new Paragraph(new Run(text)) { TextAlignment = alignment };
            TableCell cell = new TableCell(paragraph);
            cell.BorderBrush = Brushes.Black;
            cell.BorderThickness = new Thickness(1);
            cell.Padding = new Thickness(5);
            row.Cells.Add(cell);
        }

      
        public async Task<bool> ReporteXCDetalle(List<Cuenta> cuentas)
        {
            try
            {
                if (cuentas == null || cuentas.Count == 0)
                {
                    MessageBox.Show("No hay cuentas para generar el reporte.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                // Crear el documento de impresión
                FlowDocument doc = new FlowDocument
                {
                    PageHeight = 1056, // Tamaño carta
                    PageWidth = 816,
                    PagePadding = new Thickness(50),
                    ColumnWidth = 816
                };
                List<FlowDocument> flowDocuments = new List<FlowDocument>();
                Paragraph NombreComercial = new Paragraph(new Run("COOPGAFAR"))
                {
                    FontSize = 30,
                    TextAlignment = TextAlignment.Center,
                    FontWeight = FontWeights.ExtraBold,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                // Añadir título principal
                Paragraph title = new Paragraph(new Run("Reporte de aportación Socios"))
                {
                    FontSize = 20,
                    TextAlignment = TextAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 5)
                };

                Paragraph subtitle = new Paragraph(new Run("Fecha: " + DateTime.Now.ToString("dd MMMM yyyy")))
                {
                    FontSize = 12,
                    TextAlignment = TextAlignment.Center,
                    FontWeight = FontWeights.Normal,
                    Margin = new Thickness(0, 0, 0, 20)
                };

               // doc.Blocks.Add(NombreComercial);
               // doc.Blocks.Add(title);
               // doc.Blocks.Add(subtitle);

                bool primeraCuenta = true;

                // Recorremos cada cuenta, creando una sección por cuenta y forzando salto de página antes de cada sección (menos la primera)
                foreach (var cuenta in cuentas)
                {
                    Section seccionCuenta = new Section();
                    if (!primeraCuenta)
                    {
                        seccionCuenta.BreakPageBefore = true;
                    }
                    primeraCuenta = false;
                    
                    // Encabezado de la cuenta
                    Paragraph encabezadoCuenta = new Paragraph(new Run($"Cuenta: {cuenta.NumeroCuenta}    Titular: {cuenta.Titular?.Nombre} {cuenta.Titular?.Apellido}"))
                    {
                        FontSize = 14,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 10)
                    };
                    Paragraph NombreComercial2 = new Paragraph(new Run("COOPGAFAR"))
                    {
                        FontSize = 30,
                        TextAlignment = TextAlignment.Center,
                        FontWeight = FontWeights.ExtraBold,
                        Margin = new Thickness(0, 0, 0, 10)
                    };

                    // Añadir título principal
                    Paragraph title2 = new Paragraph(new Run("Reporte de aportación Socios"))
                    {
                        FontSize = 20,
                        TextAlignment = TextAlignment.Center,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 5)
                    };
                    Paragraph subtitle2 = new Paragraph(new Run("Montos Correspondientes a Fecha: " + DateTime.Now.ToString("dd /MM/ yyyy")))
                    {
                        FontSize = 12,
                        TextAlignment = TextAlignment.Center,
                        FontWeight = FontWeights.Normal,
                        Margin = new Thickness(0, 0, 0, 10)
                    };
                    seccionCuenta.Blocks.Add(NombreComercial2);
                    seccionCuenta.Blocks.Add(title2);
                    seccionCuenta.Blocks.Add(subtitle2);
                    seccionCuenta.Blocks.Add(encabezadoCuenta);
                    
                    // Crear tabla para la cuenta
                    Table table = new Table { CellSpacing = 0 };

                    // Definir columnas con anchos específicos (mismos anchos usados originalmente)
                    table.Columns.Add(new TableColumn() { Width = new GridLength(55) });  // ID
                    table.Columns.Add(new TableColumn() { Width = new GridLength(70) }); // Fecha
                    table.Columns.Add(new TableColumn() { Width = new GridLength() }); // Nombre y Apellido
                    table.Columns.Add(new TableColumn() { Width = new GridLength(70) }); // Monto
                    table.Columns.Add(new TableColumn() { Width = new GridLength(70) }); // Descripción
                    table.Columns.Add(new TableColumn() { Width = new GridLength(100) }); // Número de Cuenta

                    // Encabezado de columnas (por cuenta)
                    TableRowGroup headerGroup = new TableRowGroup();
                    TableRow headerRow = new TableRow();
                    List<string> columnas = new() { "ID", "Fecha", "Descripción", "Crédito", "Debito", "Monto Total" };
                    foreach (string colum in columnas)
                    {
                        TableCell cell = new TableCell(new Paragraph(new Run(colum)))
                        {
                            Background = Brushes.LightGray,
                            FontWeight = FontWeights.Bold,
                            BorderBrush = Brushes.Black,
                            FontSize = 12,
                            TextAlignment = TextAlignment.Center,
                            BorderThickness = new Thickness(1),
                            Padding = new Thickness(3)
                        };
                        headerRow.Cells.Add(cell);
                    }
                    headerGroup.Rows.Add(headerRow);
                    table.RowGroups.Add(headerGroup);

                    // Datos de transacciones de la cuenta
                    TableRowGroup dataGroup = new TableRowGroup();

                    var transacciones = cuenta.Transacciones?.Where(x => x.Nula == false).ToList() ?? new List<Transaccion>();
                    decimal totalCuenta = 0m;

                    foreach (var item in transacciones)
                    {
                        TableRow row = new TableRow { FontSize = 10 };
                        AddCell(row, item.Id.ToString(), TextAlignment.Center);
                        AddCell(row, item.Fecha.ToString("dd/MM/yyyy"), TextAlignment.Left);
                        AddCell(row, item.Descripcion + " " + item.Cuenta?.Titular?.Apellido, TextAlignment.Left);
                        AddCell(row, item.MontoCredito.ToString("n2"), TextAlignment.Right);
                        AddCell(row, item.MontoDebito.ToString("n2"), TextAlignment.Right);
                        AddCell(row, item.Monto.ToString("n2"), TextAlignment.Right);

                        dataGroup.Rows.Add(row);

                        // Sumar al total de la cuenta (suponiendo que Monto representa crédito - débito como ya lo manejan en Saldo)
                        totalCuenta += item.MontoCredito - item.MontoDebito;
                    }

                    table.RowGroups.Add(dataGroup);

                    seccionCuenta.Blocks.Add(table);

                    // Añadir total de la cuenta
                    Paragraph total = new Paragraph(new Run("Balance Total: " + totalCuenta.ToString("n2")))
                    {
                        FontSize = 12,
                        FontWeight = FontWeights.Bold,
                        TextAlignment = TextAlignment.Right,
                        Margin = new Thickness(0, 10, 0, 0)
                    };
                    seccionCuenta.Blocks.Add(total);

                    // Añadir la sección al documento
                    doc.Blocks.Add(seccionCuenta);
                }
                 

                // Configurar e iniciar la impresión
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    IDocumentPaginatorSource idpSource = doc;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Reporte de aportación Socios");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir: " + ex.Message, "Error");
                return false;
            }
        }

       
    }
}
