using MaterialDesignThemes.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para PInventarios.xaml
    /// </summary>
    public partial class PInventarios : Page
    {
        IProducto produc = new ProductoService();
        public PInventarios()
        {
            InitializeComponent();
            Cargar();
        }
        public async void Cargar() {
            var lis= await produc.OptenerContInventario();
            data.ItemsSource = lis.OrderByDescending(x => x.Id);
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem is ContInventario selectedInventario)
            {
                if (selectedInventario.Aplicado)
                {
                    MessageBox.Show("El inventario ya ha sido aplicado.");
                    return;
                }
                var result = MessageBox.Show("¿Desea aplicar este inventario?", "Confirmar Aplicación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                        await produc.AplicarInventario(selectedInventario.Id);
                    Cargar(); // Actualizar la lista después de aplicar el inventario
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un inventario para aplicar.");
            }
              
        }
    }
}
