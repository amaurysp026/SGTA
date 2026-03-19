using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
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
using System.Windows.Shapes;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para VBuscarProducto.xaml
    /// </summary>
    public partial class VBuscarProducto : Window
    {
        ICategorias categoria = new CategoriaService();
        public Producto ProductoSeleccionado { get; set; }
        public VBuscarProducto()
        {
            InitializeComponent();
            Cargar();
        }
        private async void Cargar()
        {
            data.ItemsSource = null;
            data.ItemsSource = await categoria.ObtenerTodosProductos();
            txtbuscar.Focus();
        }

        private void data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (data.SelectedItem != null)
            {
                ProductoSeleccionado = (Producto)data.SelectedItem;
                this.DialogResult = true;
                this.Close();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void data_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
