using SFCH.IService;
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
    /// Lógica de interacción para PFrezzer.xaml
    /// </summary>
    public partial class PFrezzer : Window
    {
       // IRecepcion recepcion = new Logica.RecepcionService();
        public PFrezzer()
        {
            InitializeComponent();
            Window_Loaded();
        }
        private async void Window_Loaded()
        {
            try
            {
               // var freezers = await recepcion.ObtenerFreezersAsync();
             //   data.ItemsSource = freezers;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los freezers: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            VNewFrezzer vNewFrezzer = new VNewFrezzer();
            if (vNewFrezzer.ShowDialog()==true)
            {
              // if(await recepcion.GuardarFreezer(vNewFrezzer.freezerrespuesta)==true)
               // {
               //    Window_Loaded();
               // }
                ;
            }
        }

        private async void btneditar_Click(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem is f fr) {
                VNewFrezzer vNewFrezzer = new VNewFrezzer(fr);
               // vNewFrezzer.freezerrespuesta = fr;
                if (vNewFrezzer.ShowDialog() == true)
                {

                  //  if (await recepcion.GuardarFreezer(vNewFrezzer.freezerrespuesta) == true)
                  //  {
                    //    Window_Loaded();
                   // }
                    //;
                }
            }
            
        }

        private void btndesactivar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btninforme_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
