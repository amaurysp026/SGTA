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
    /// Lógica de interacción para VBuscarRecepcion.xaml
    /// </summary>
    public partial class VBuscarRecepcion : Window
    {
        IRecepcion recepcion = new Logica.RecepcionService();
        public RecepcionLeche RecepcionLeche { get; set; }=new RecepcionLeche();
        public VBuscarRecepcion()
        {
            InitializeComponent();
            Cargar();
        }
        private async void Cargar()
        {
           
            var lista = await recepcion.ObtenerRecepcionesLecheAsync();
            data.ItemsSource = lista;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (data.SelectedItem != null)
            {
                RecepcionLeche = (RecepcionLeche)data.SelectedItem;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
