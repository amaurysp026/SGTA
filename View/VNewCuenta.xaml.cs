using MaterialDesignColors.Recommended;
using SFCH.IService;
using SFCH.Logica;
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
using System.Windows.Shapes;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para VNewCuenta.xaml
    /// </summary>
    public partial class VNewCuenta : Window
    {
        private ICuenta Icuenta = new CuentaService();
        private IHerramientas herramientas =new HerramientasService();
        public Cuenta cuenta=new Cuenta();
        public VNewCuenta()
        {
            InitializeComponent();
            Cargar();
            this.DataContext = cuenta;
        }
        public async void Cargar()
        {
            cbTtitular.ItemsSource = await herramientas.ObtenerEntidades();
            cbTCuenta.ItemsSource = await herramientas.ObtenerTiposCuenta();

        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            

            if (cuenta.Titular==null||cuenta.Titular.Id == 0)
            {
                MessageBox.Show("Es necesario seleccionar un titular para la cuenta", "Aviso!", MessageBoxButton.OK,MessageBoxImage.Information);
                return;
            }
            if (cuenta.TipoCuenta==null||cuenta.TipoCuenta.Id==0)
            {
                MessageBox.Show("Es necesario seleccionar un Tipo para la cuenta", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (await Icuenta.GuardarCuenta(cuenta))
            {
                this.DialogResult = true;
            }
            else
            {
                this.DialogResult = false;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TipoEntidad tipo = cbTtitular.SelectedItem as TipoEntidad;
            VBuscarPersona vBuscarPersona = new VBuscarPersona(tipo);
            if (vBuscarPersona.ShowDialog() == true)
            {
                cuenta.Titular = vBuscarPersona.PersonaSelecionada;
                txtNombre.Text = cuenta.Titular.Nombre +" " + cuenta.Titular.Apellido;
            }
        }
    }
}
