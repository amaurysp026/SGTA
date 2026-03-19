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
    /// Lógica de interacción para VBuscarProveedores.xaml
    /// </summary>
    public partial class VBuscarProveedores : Window
    {
        public Proveedor ProveedorSeleccionado { get; set; }
        private IProveedor pro= new ProveedorService();
        public VBuscarProveedores()
        {
            InitializeComponent();
            Cargar();
        }
        private async void Cargar()
        {
                        var lista= await pro.ObtenerProveedores();
            data.ItemsSource = lista;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (data.SelectedItem != null)
            {
                ProveedorSeleccionado = (Proveedor)data.SelectedItem;
                this.DialogResult = true;
                this.Close();
            }

        }
    }
}
