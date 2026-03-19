using Microsoft.EntityFrameworkCore;
using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SFCH
{
    /// <summary>
    /// Lógica de interacción para VReportes.xaml
    /// </summary>
    public partial class VReportes : Window
    {
        public ICuenta cuenta=new CuentaService();
        List<Transaccion> transacciones = new List<Transaccion>();  
        public VReportes()
        {
            InitializeComponent();
            using (var db=new Conexion())

            {
                transacciones= db.Transacciones.Include(x => x.Cuenta).ThenInclude(x => x.Titular).Where(x=>x.Nula==false).ToList();
                txttotal.Text = "Total: RD$ " + transacciones.Sum(x => x.Monto).ToString("N2");
                Hasta.SelectedDate=transacciones.LastOrDefault().Fecha;
                Desde.SelectedDate=transacciones.FirstOrDefault().Fecha;
                data.ItemsSource = transacciones;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            data.ItemsSource = null;
            var transaccionesFiltradas = transacciones.Where(t => 
                t.Fecha >= Desde.SelectedDate && 
                t.Fecha <= Hasta.SelectedDate).OrderBy(x=>x.Id).ToList();

                txttotal.Text = "Total: RD$ "+ transaccionesFiltradas.Sum(x => x.Monto).ToString("C");
            data.ItemsSource = transaccionesFiltradas;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                // Crear el documento de impresión
                FlowDocument doc = new FlowDocument();
                doc.PageHeight = 1056; // Tamaño carta
                doc.PageWidth = 816;
                doc.PagePadding = new Thickness(50);
                doc.ColumnWidth = 816;

                // Añadir título
                Paragraph title = new Paragraph(new Run("Reporte de Transacciones COOPGAFAR"));
                title.FontSize = 20;
                title.TextAlignment = TextAlignment.Center;
                title.FontWeight = FontWeights.Bold;
                title.Margin = new Thickness(0, 0, 0, 20);
                doc.Blocks.Add(title);

                Paragraph subtitle = new Paragraph(new Run("Desde: "+Desde.SelectedDate.Value.ToString("dd/MM/yyyy")+"  "+" Hasta:"+ Hasta.SelectedDate.Value.ToString("dd/MM/yyyy")));
                title.FontSize = 20;
                title.TextAlignment = TextAlignment.Center;
                title.FontWeight = FontWeights.Bold;
                title.Margin = new Thickness(0, 0, 0, 20);
             //   doc.Blocks.Add(title);
                doc.Blocks.Add(subtitle);
                // Crear tabla
                Table table = new Table();
                table.CellSpacing = 0;
               
                // Definir columnas con anchos específicos
                table.Columns.Add(new TableColumn() { Width = new GridLength(8) });  // ID
table.Columns.Add(new TableColumn() { Width = new GridLength(82) }); // Fecha
table.Columns.Add(new TableColumn() { Width = new GridLength(260) }); // Nombre y Apellido
table.Columns.Add(new TableColumn() { Width = new GridLength(100) }); // Monto
table.Columns.Add(new TableColumn() { Width = new GridLength(200) }); // Descripción
table.Columns.Add(new TableColumn() { Width = new GridLength(100) }); // Número de Cuenta
                //el total debe de ser 750
                // Crear encabezados
                TableRowGroup headerGroup = new TableRowGroup();
                TableRow headerRow = new TableRow();
                
                foreach (DataGridColumn column in data.Columns)
                {
                    TableCell cell = new TableCell(new Paragraph(new Run(column.Header.ToString())));
                    cell.Background = Brushes.LightGray;
                    cell.FontWeight = FontWeights.Bold;
                    cell.BorderBrush = Brushes.Black;
                    cell.BorderThickness = new Thickness(1);
                    cell.Padding = new Thickness(5);
                    headerRow.Cells.Add(cell);
                }
                headerGroup.Rows.Add(headerRow);
                table.RowGroups.Add(headerGroup);

                // Añadir datos
                TableRowGroup dataGroup = new TableRowGroup();
                foreach (Transaccion item in data.Items)
                {
                    TableRow row = new TableRow();
                    
                    // Añadir las celdas según las propiedades visibles en el DataGrid
                    AddCell(row, item.Id.ToString());
                    AddCell(row, item.Fecha.ToString("dd/MM/yyyy"));
                    AddCell(row, item.Cuenta.Titular.Nombre);
                    AddCell(row, item.Monto.ToString("n2"), TextAlignment.Right); // Alineación derecha para el monto
                    AddCell(row, item.Descripcion);
                    AddCell(row, item.Cuenta?.NumeroCuenta.ToString() ?? "");
                    
                    dataGroup.Rows.Add(row);
                }
                table.RowGroups.Add(dataGroup);
                doc.Blocks.Add(table);

                // Añadir total al final
                Paragraph total = new Paragraph(new Run(txttotal.Text));
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
                    printDialog.PrintDocument(idpSource.DocumentPaginator, "Reporte de Transacciones COOPGAFAR");
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
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
                Paragraph title = new Paragraph(new Run("Reporte de Transacciones"));
                title.FontSize = 20;
                title.TextAlignment = TextAlignment.Center;
                title.FontWeight = FontWeights.Bold;
                title.Margin = new Thickness(0, 0, 0, 20);

                Paragraph subtitle = new Paragraph(new Run("Desde: " + Desde.SelectedDate.Value.ToString("dd/MM/yyyy") + "  " + " Hasta:" + Hasta.SelectedDate.Value.ToString("dd/MM/yyyy")));
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
                table.Columns.Add(new TableColumn() { Width = new GridLength(55) });  // ID
table.Columns.Add(new TableColumn() { Width = new GridLength(70) }); // Fecha
table.Columns.Add(new TableColumn() { Width = new GridLength(225) }); // Nombre y Apellido
table.Columns.Add(new TableColumn() { Width = new GridLength(70) }); // Monto
table.Columns.Add(new TableColumn() { Width = new GridLength(215) }); // Descripción
table.Columns.Add(new TableColumn() { Width = new GridLength(80) }); // Número de Cuenta

                // Crear encabezados
                TableRowGroup headerGroup = new TableRowGroup();
                TableRow headerRow = new TableRow();

                foreach (DataGridColumn column in data.Columns)
                {
                    TableCell cell = new TableCell(new Paragraph(new Run(column.Header.ToString())));
                    cell.Background = Brushes.LightGray;
                    cell.FontWeight = FontWeights.Bold;
                    cell.BorderBrush = Brushes.Black;
                    cell.FontSize = 12;
                    cell.BorderThickness = new Thickness(1);
                    cell.Padding = new Thickness(5);
                    headerRow.Cells.Add(cell);
                }
                headerGroup.Rows.Add(headerRow);
                table.RowGroups.Add(headerGroup);

                // Añadir datos
                TableRowGroup dataGroup = new TableRowGroup();
                foreach (Transaccion item in data.Items)
                {
                    TableRow row = new TableRow();
                    row.FontSize = 10;
                    // Añadir las celdas según las propiedades visibles en el DataGrid
                    AddCell(row, item.Id.ToString(), TextAlignment.Center);
                    AddCell(row, item.Fecha.ToString("dd/MM/yyyy"),TextAlignment.Left);
                    AddCell(row, item.Cuenta?.Titular?.Nombre+ " " + item.Cuenta?.Titular.Apellido,TextAlignment.Left);
                    AddCell(row, item.Monto.ToString("n2"), TextAlignment.Right); // Alineación derecha para el monto
                    AddCell(row, item.Descripcion, TextAlignment.Left);
                    AddCell(row, item.Cuenta?.NumeroCuenta.ToString() ?? "", TextAlignment.Center);

                    dataGroup.Rows.Add(row);
                }
                table.RowGroups.Add(dataGroup);
                doc.Blocks.Add(table);

                // Añadir total al final
                Paragraph total = new Paragraph(new Run("Total Transacciones: "+transacciones.Count()+"  |  "+ txttotal.Text));
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
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
