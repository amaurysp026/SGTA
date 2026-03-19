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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para PPersona.xaml
    /// </summary>
    public partial class PPersona : Page
    {
        private IPersona persona = new PersonaService();
        public  PPersona(TipoEntidad tipoEntidad)
        {
            InitializeComponent();
         
            cargar();

        }
        public async void cargar()
        {
            List<Persona> p = await persona.ObtenerPersonas();
            data.ItemsSource = p;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem is Persona persona)
            {
                VNewPersona vNewPersona = new VNewPersona(persona);
                vNewPersona.ShowDialog();
            }
            else
            {
                MessageBox.Show("Es necesario seleccionar una persona");
            }
        }

        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
