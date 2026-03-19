using SFCH.IService;
using SFCH.Logica;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Lógica de interacción para PCompras.xaml
    /// </summary>
    public partial class PCompras : Page
    {
        private ICompra ucompra=new CompraService();
        public PCompras()
        {
            InitializeComponent();
            Cargar();
        }
        private async void Cargar()
        {
            var lista= await ucompra.ObtenerCompras();
            data.ItemsSource = lista;
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var compraSeleccionada = data.SelectedItem as Model.Compra;
            if (compraSeleccionada != null)
            {
               await ucompra.AnularCompra(compraSeleccionada);
                Cargar();
            }

        }
    }
}
