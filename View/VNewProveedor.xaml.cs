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
    /// Lógica de interacción para VNewProveedor.xaml
    /// </summary>
    public partial class VNewProveedor : Window
    {
        private Proveedor proveedor { get; set; }= new Proveedor();
        private IProveedor proveedorService = new ProveedorService();
        public VNewProveedor()
        {
            InitializeComponent();
           
          
            DataContext = proveedor;


        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
          var res=  await proveedorService.GuardarProveedor(proveedor);
            if (res)
            {
                //MessageBox.Show("Proveedor guardado con exito");
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al guardar el proveedor");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
