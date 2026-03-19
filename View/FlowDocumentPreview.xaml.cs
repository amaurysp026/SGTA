using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms.Integration;
namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para FlowDocumentPreview.xaml
    /// </summary>
    public partial class FlowDocumentPreview : Window
    {
        public FlowDocument documento { get; set;  }
        private ElementHost host;
        private System.Windows.Controls.FlowDocumentScrollViewer wpfViewer;
        public FlowDocumentPreview()
        {
            InitializeComponent();
            CrearDocumentoSimple();
            InicializarWPFHost();
        }

        private void CrearDocumentoSimple()
        {
            // Crear un nuevo FlowDocument
            FlowDocument document = new FlowDocument();

            // Configurar propiedades del documento
            document.FontFamily = new System.Windows.Media.FontFamily("Segoe UI");
            document.FontSize = 12;
            document.TextAlignment = TextAlignment.Left;
            document.PagePadding = new Thickness(50);
            document.ColumnWidth = 400;

            // Crear contenido
            Paragraph titulo = new Paragraph(new Run("Documento de Ejemplo"))
            {
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };

            Paragraph contenido = new Paragraph();
            contenido.Inlines.Add(new Run("Este es un ejemplo de cómo previsualizar un FlowDocument en WPF. ")
            {
                Foreground = System.Windows.Media.Brushes.Blue
            });
            contenido.Inlines.Add(new LineBreak());
            contenido.Inlines.Add(new Run("Podemos agregar texto con diferentes estilos y formatos."));

            // Agregar elementos al documento
            document.Blocks.Add(titulo);
            document.Blocks.Add(contenido);

            // Agregar lista
            List lista = new List();
            lista.ListItems.Add(new ListItem(new Paragraph(new Run("Elemento 1 de la lista"))));
            lista.ListItems.Add(new ListItem(new Paragraph(new Run("Elemento 2 de la lista"))));
            lista.ListItems.Add(new ListItem(new Paragraph(new Run("Elemento 3 de la lista"))));
            document.Blocks.Add(lista);

            // Asignar el documento al visor
            flowDocumentViewer.Document = document;
        }

        private void btnCargar_Click(object sender, RoutedEventArgs e)
        {
            // Método para cargar un documento desde archivo o XAML
            CargarDocumentoDesdeXaml();
        }

        private void CargarDocumentoDesdeXaml()
        {
            string xamlContent = @"<FlowDocument 
                xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                FontFamily='Segoe UI' FontSize='12' PagePadding='50'>
                <Paragraph FontSize='24' FontWeight='Bold' TextAlignment='Center'>
                    Documento Cargado desde XAML
                </Paragraph>
                <Paragraph>
                    Este documento fue creado dinámicamente desde código XAML.
                </Paragraph>
                <Table>
                    <Table.RowGroups>
                        <TableRowGroup>
                            <TableRow>
                                <TableCell><Paragraph>Celda 1</Paragraph></TableCell>
                                <TableCell><Paragraph>Celda 2</Paragraph></TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell><Paragraph>Celda 3</Paragraph></TableCell>
                                <TableCell><Paragraph>Celda 4</Paragraph></TableCell>
                            </TableRow>
                        </TableRowGroup>
                    </Table.RowGroups>
                </Table>
            </FlowDocument>";

            using (var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xamlContent)))
            {
                FlowDocument doc = (FlowDocument)System.Windows.Markup.XamlReader.Load(stream);
                flowDocumentViewer.Document = doc;
            }
        }
        private void InicializarWPFHost()
        {

            

            // Crear el visor WPF
            wpfViewer = new System.Windows.Controls.FlowDocumentScrollViewer();
            host.Child = wpfViewer;

            // Crear documento
            FlowDocument doc = new FlowDocument();
            doc.Blocks.Add(new Paragraph(new Run("Documento en Windows Forms")));
            wpfViewer.Document = documento;
        }
        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            // Mostrar vista previa de impresión
            System.Windows.Controls.PrintDialog printDialog = new System.Windows.Controls.PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                // Crear un DocumentPaginator para imprimir
                IDocumentPaginatorSource paginatorSource = flowDocumentViewer.Document;
                printDialog.PrintDocument(paginatorSource.DocumentPaginator, "Previsualización FlowDocument");
            }
        }
    }
}
