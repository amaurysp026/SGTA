using SFCH.Controller;
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

namespace SGTA.View
{
    /// <summary>
    /// Lógica de interacción para PRecepVehiculo.xaml
    /// </summary>
    public partial class PRecepVehiculo : Page
    {
        public PRecepVehiculo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         MessageBox.Show(   Util.HashPassword("Data Source=AMAURYS\\SERVER;Initial Catalog=SFCHDB;TrustServerCertificate=True;Integrated Security=True;"));
        }
    }
}
