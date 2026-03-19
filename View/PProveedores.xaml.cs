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
    /// Lógica de interacción para PProveedores.xaml
    /// </summary>
    public partial class PProveedores : Page
    {
        private IProveedor proveedorService = new ProveedorService();
        public PProveedores()
        {
            InitializeComponent();
            Page_Loaded(null, null);
        }
       private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            data.ItemsSource = await proveedorService.ObtenerProveedores();
        }
    }
}
