using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para PCuentas.xaml
    /// </summary>
    public partial class PCuentas : Page
    {
        private ICuenta cuenta = new CuentaService();
        public PCuentas()
        {
            InitializeComponent();
           cargar();
        }
        public async void cargar()
        {
            var cunt= await cuenta.ObtenerCuentas();
            data.ItemsSource = null;
            data.ItemsSource = cunt;
            txttotal.Text = "Suma Total Saldos: " + cunt.Sum(x => x.Saldo).ToString("c");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (data2.Visibility==Visibility.Visible)
            {
                data2.Visibility = Visibility.Collapsed;
            }
            else
            {
                data2.Visibility = Visibility.Visible;
                
            }
        }

        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data.SelectedItem is Cuenta cu)
            {
                data2.ItemsSource = cu.Transacciones;

            }
        }

        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem is Cuenta cuent)
            {
                if (MessageBox.Show("Deseas desactivar la cuenta No."+cuent.NumeroCuenta+", Titular "+cuent.Titular.Nombre+". Deseas continuar?","Aviso!",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                {
                  await  cuenta.DesactivarCuenta(cuent);
                    var cunt = await cuenta.ObtenerCuentas();
                    data.ItemsSource = null;
                    data.ItemsSource = cunt;
                    txttotal.Text = "Suma Total Saldos: " + cunt.Sum(x => x.Saldo).ToString("c");
                }
            }

        }

        private async void btnRecalcular_Click(object sender, RoutedEventArgs e)
        {

            if ( await cuenta.CalcularTodoTotal())
            {
              //  MessageBox.Show("Cuentas Recalculadas");

            }
            else
            {
               // MessageBox.Show("Cuentas no recalculadas");
            }


                var cunt = await cuenta.ObtenerCuentas();
            data.ItemsSource = null;
            data.ItemsSource = cunt;
            txttotal.Text = "Suma Total Saldos: " + cunt.Sum(x => x.Saldo).ToString("c");
        }

        private  void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem is Cuenta cuent)
            {
                 cuenta.CalcularTotal(cuent);
               // MessageBox.Show("etrn");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Crear el documento de impresión
                FlowDocument doc = new FlowDocument();
                doc.PageHeight = 1056; // Tamaño carta
                doc.PageWidth = 816;
                doc.PagePadding = new Thickness(50);
                doc.ColumnWidth = 816;

                Paragraph NombreComercial = new Paragraph(new Run("COOPGAFAR"));
                NombreComercial.FontSize = 30;
                NombreComercial.TextAlignment = TextAlignment.Center;
                NombreComercial.FontWeight = FontWeights.ExtraBold;
                NombreComercial.Margin = new Thickness(0, 0, 0, 20);

                // Añadir título
                Paragraph title = new Paragraph(new Run("Reporte de Cuentas"));
                title.FontSize = 20;
                title.TextAlignment = TextAlignment.Center;
                title.FontWeight = FontWeights.Bold;
                title.Margin = new Thickness(0, 0, 0, 20);

                Paragraph subtitle = new Paragraph(new Run("Fecha de Reporte: "  +DateTime.Now.ToString("dd/MM/yyyy") ));
                title.FontSize = 20;
                title.TextAlignment = TextAlignment.Center;
                title.FontWeight = FontWeights.Bold;
                title.Margin = new Thickness(0, 0, 0, 20);
                //   doc.Blocks.Add(title);
                doc.Blocks.Add(NombreComercial);
                doc.Blocks.Add(title);
                doc.Blocks.Add(subtitle);

                // Crear tabla
                Table table = new Table();
                table.CellSpacing = 0;

                // Definir columnas con anchos específicos
                table.Columns.Add(new TableColumn() { Width = new GridLength(50) });  // ID
                table.Columns.Add(new TableColumn() { Width = new GridLength(70) }); // Fecha
                table.Columns.Add(new TableColumn() { Width = new GridLength() }); // Nombre y Apellido
                table.Columns.Add(new TableColumn() { Width = new GridLength(75) }); // Monto
                table.Columns.Add(new TableColumn() { Width = new GridLength(80) }); // Descripción
                table.Columns.Add(new TableColumn() { Width = new GridLength(60) }); // Número de Cuenta

                // Crear encabezados
                TableRowGroup headerGroup = new TableRowGroup();
                TableRow headerRow = new TableRow();
               

                string[] headers = new[] { "ID", "Numero Cuneta", "Nombre Titular", "Can. Transacciónes", "Saldo a la fecha", "Moneda" };
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

                //foreach (DataGridColumn column in data.Columns)
                //{
                //    TableCell cell = new TableCell(new Paragraph(new Run(column.Header.ToString())));
                //    cell.Background = Brushes.LightGray;
                //    cell.FontWeight = FontWeights.Bold;
                //    cell.BorderBrush = Brushes.Black;
                //    cell.FontSize = 12;
                //    cell.BorderThickness = new Thickness(1);
                //    cell.Padding = new Thickness(5);
                //    headerRow.Cells.Add(cell);
                //}
                headerGroup.Rows.Add(headerRow);
                table.RowGroups.Add(headerGroup);
                decimal totalSaldo = 0;
                // Añadir datos
                TableRowGroup dataGroup = new TableRowGroup();
                foreach (Cuenta item in data.Items)
                {
                    TableRow row = new TableRow();
                    row.FontSize = 10;
                    // Añadir las celdas según las propiedades visibles en el DataGrid
                    AddCell(row, item.Id.ToString(), TextAlignment.Center);
                    AddCell(row, item.NumeroCuenta.ToString(), TextAlignment.Right);
                    AddCell(row, item.Titular?.Nombre + " " + item?.Titular.Apellido, TextAlignment.Left);
                    AddCell(row, item.Transacciones.Count(x=>!x.Nula).ToString(), TextAlignment.Center); // Alineación derecha para el monto
                    AddCell(row, item.Saldo.ToString("C"), TextAlignment.Right);
                    AddCell(row, item.Moneda ?? "", TextAlignment.Center);
                     totalSaldo += item.Saldo;
                    dataGroup.Rows.Add(row);
                }
                table.RowGroups.Add(dataGroup);
                doc.Blocks.Add(table);

                // Añadir total al final
                Paragraph total = new Paragraph(new Run("Documento para uso interno de la institución. " +   "  |  Total Cuentas: " + data.Items.Count+"   |  Total Saldos: "+totalSaldo.ToString("N2")));
                total.FontSize = 14;
                total.FontWeight = FontWeights.Bold;
                total.TextAlignment = TextAlignment.Right;
                total.Margin = new Thickness(0, 20, 0, 0);
                doc.Blocks.Add(total);




                // Configurar e iniciar la impresión
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    IDocumentPaginatorSource idpSource = doc;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Reporte de Transacciones");
                 
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            cuenta.ReporteXCDetalle( data.Items.Cast<Cuenta>().ToList());
        }
    }
}
