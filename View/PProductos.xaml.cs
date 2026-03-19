using SFCH.IService;
using SFCH.Logica;
using SFCH.Migrations;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para PProductos.xaml
    /// </summary>
    public partial class PProductos : Page
    {
        IProducto producto=new ProductoService();
        public PProductos()
        {
            InitializeComponent();

            _= Cargar();
        }
       public async Task Cargar()
        {
            try
            {
                var productos= await producto.ObtenerProductos();
                data.ItemsSource = productos;
                txtTinventario.Text = "Valor Total de Inventario: " + productos.Sum(x=>x.ValorInventario).ToString("C");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Mbtneditarp_Click(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem is Producto prod)
            {
                VNewProducto vNewProducto = new VNewProducto(prod);
                vNewProducto.ShowDialog();
                _ = Cargar();
            }
            else
            {
                MessageBox.Show("Seleccione un producto para completar la acción","Aviso",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
           

        }

        private async void mBtnInventarioajus_Click(object sender, RoutedEventArgs e)
        {
          await producto.MovAjusteInventario(3019, 3, "Prueba de ajuste");

        }

        private void Mbtver_Click(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem is Producto prod)
            {
                VNewProducto vNewProducto = new VNewProducto(prod,true);
                vNewProducto.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione un producto para completar la acción", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }
    }
}
