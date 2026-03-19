using SFCH.Controller;
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

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para VFacAbiertas.xaml
    /// </summary>
    public partial class VFacAbiertas : Window
    {
        ITurno turno = new TurnoService();
        IFactura factura = new FacturaService();
        public Factura FacturaSelecionada { get; set; }
        string cantabiertas;
        public VFacAbiertas()
        {
            InitializeComponent();
            _ = Cargar();

        }
        private async Task Cargar()
        {
            var tru = await turno.OptenerFacturasAbiertas(SesionUsuario.TurnoActual);
            data.ItemsSource = tru.OrderByDescending(x=>x.Id);
            cantabiertas = tru.Count().ToString() + " Facturas Abiertas";
            txtTitulo.Text = cantabiertas;
        }

        private void BtnCerrarFactura_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRefrescar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (data.SelectedItem != null)
            {
                FacturaSelecionada  = (SFCH.Model.Factura)data.SelectedItem;
                DialogResult=true;
                this.Close();
            }
        }

        // Pseudocódigo detallado:
        // 1. Obtener el texto del textbox y limpiarlo (Trim).
        // 2. Obtener la lista actual de facturas abiertas llamando a turno.OptenerFacturasAbiertas.
        // 3. Si el texto está vacío: mostrar la lista completa ordenada por Id descendente y actualizar el título con el conteo.
        // 4. Determinar si la búsqueda es por número o por nombre:
        //    - Si el primer carácter es dígito: filtrar por Id que empiece con el texto (ToString().StartsWith).
        //    - Si el primer carácter es letra: filtrar por el nombre del cliente (Cliente.Nombre) usando Contains (case-insensitive).
        //    - Si es otro carácter: intentar buscar tanto por nombre como por Id (combinado).
        // 5. Ordenar los resultados por Id descendente, asignar a data.ItemsSource y actualizar el título con el conteo de resultados.
        // 6. Capturar excepciones silenciosamente (o loguear) para no romper la UI durante la escritura rápida.
        private async void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var texto = txtBuscar.Text?.Trim() ?? string.Empty;
                var todas = await turno.OptenerFacturasAbiertas(SesionUsuario.TurnoActual);

                if (string.IsNullOrEmpty(texto))
                {
                    var allList = todas.OrderByDescending(x => x.Id).ToList();
                    data.ItemsSource = allList;
                    txtTitulo.Text = $"{allList.Count} Facturas Abiertas";
                    return;
                }

                char first = texto[0];
                IEnumerable<Factura> filtradas;

                if (char.IsDigit(first))
                {
                    // Buscar por número/Id de factura (coincidencia por prefijo)
                    filtradas = todas.Where(f => f.Id.ToString().StartsWith(texto, StringComparison.OrdinalIgnoreCase));
                }
                else if (char.IsLetter(first))
                {
                    // Buscar por nombre de cliente (case-insensitive, contains)
                    var lower = texto.ToLowerInvariant();
                    filtradas = todas.Where(f =>
                        !string.IsNullOrEmpty(f?.NombreCliente) &&
                        f.NombreCliente.ToLowerInvariant().Contains(lower));
                }
                else
                {
                    // Caracteres mixtos: intentar ambas búsquedas
                    var lower = texto.ToLowerInvariant();
                    filtradas = todas.Where(f =>
                        (!string.IsNullOrEmpty(f?.NombreCliente) && f.NombreCliente.ToLowerInvariant().Contains(lower))
                        || f.Id.ToString().StartsWith(texto, StringComparison.OrdinalIgnoreCase));
                }

                var resultList = filtradas.OrderByDescending(x => x.Id).ToList();
                data.ItemsSource = resultList;
                txtTitulo.Text = $"{resultList.Count} Facturas Abiertas";
            }
            catch (Exception)
            {
                // Evitar que excepciones rompan la experiencia de escritura.
                // En caso necesario, aquí se puede añadir logging.
            }
        }
    }
}
