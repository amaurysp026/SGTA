using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps;


namespace SFCH.PrintView
{
    /// <summary>
    /// Lógica de interacción para PrevDocumento.xaml
    /// </summary>
    public partial class PrevDocumento : Window
    {
        public FlowDocument documentof { get; set; }
        public PrevDocumento(FlowDocument fd)
        {
            InitializeComponent();
            documentof = fd;
            
            Width = fd.PageWidth+50;
        
            Title = "Previsualización de Documento";

            CargarDocumento();
        }
        private void CargarDocumento()
        {
            FlowDocument doc = new FlowDocument();

            Paragraph titulo = new Paragraph(new Run("Reporte de Productos"));
            titulo.FontSize = 22;
            titulo.FontWeight = FontWeights.Bold;
            titulo.Foreground = Brushes.DarkBlue;

            Paragraph contenido = new Paragraph(new Run("Este es un ejemplo simple de FlowDocument en WPF."));

            doc.Blocks.Add(titulo);
            doc.Blocks.Add(contenido);

            docViewer.Document = documentof;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Crear paginador
            IDocumentPaginatorSource paginatorSource = documentof;
            DocumentPaginator paginator = paginatorSource.DocumentPaginator;

            // Obtener impresora PDF
            LocalPrintServer printServer = new LocalPrintServer();
            PrintQueue printQueue = printServer.GetPrintQueue("Microsoft Print to PDF");

            // Crear ticket de impresión
            PrintTicket printTicket = new PrintTicket();
            printTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);

            // Asignar nombre de archivo de salida
            printQueue.UserPrintTicket = printTicket;
            printQueue.DefaultPrintTicket.OutputColor = OutputColor.Color;

            // Crear escritor
            XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(printQueue);

            // Guardar PDF automáticamente
            printQueue.CurrentJobSettings.Description = "Exportando PDF";

            writer.Write(paginator, printTicket);

            // IMPORTANTE:
            // Windows pedirá la ruta manualmente si no está configurado el OutputFileName.
            this.DialogResult = false;
        }
    }
}
