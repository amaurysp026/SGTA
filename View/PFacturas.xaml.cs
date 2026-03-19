using SFCH.Controller;
using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Lógica de interacción para PFacturas.xaml
    /// </summary>
    public partial class PFacturas : Page,INotifyPropertyChanged
    {
        public List<Factura> faturas
        {
            get { return field; }
            set
            {
                field = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs( nameof(faturas)));  
            }
        }
        IFactura Ifaturas = new FacturaService();
        public PFacturas()
        {
            InitializeComponent();
            Cargar();
            DataContext = this;
        }
        public async void Cargar()
        {
            progreso.Visibility = Visibility.Visible;
            faturas = await Ifaturas.ObtenerFacturas();
            if (SesionUsuario.Usuario.AnularFacturas==false)
            {
                                mbtnAnular.Visibility = Visibility.Collapsed;

            }
            progreso.Visibility = Visibility.Collapsed;
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void mbtnimprimirGrande_Click(object sender, RoutedEventArgs e)
        {
          
                if (data.SelectedItem is Factura fac)
                {
                    //Ifaturas.ImprimirFactura(fac);
                    Ifaturas.ImprimirFacturaGrande(fac, true);
                }
           
        }

        private void mbtnAnular_Click(object sender, RoutedEventArgs e)
        {
            if (SesionUsuario.Usuario.AnularFacturas)
            {
                if (data.SelectedItem is Factura fac)
                {
                    Ifaturas.AnularFactura(fac);
                }
            }
            else
            {
                MessageBox.Show("No tienes permisos para realizar esta acción.", "Permisos insuficientes", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void mbtnvisualizar_Click(object sender, RoutedEventArgs e)
        {

            if (data.SelectedItem is Factura fac)
            {
                Ifaturas.ImprimirFactura(fac,true);
            }
        }
    }
}
