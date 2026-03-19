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
    /// Lógica de interacción para VBuscarPersona.xaml
    /// </summary>
    public partial class VBuscarPersona : Window
    {
        private IPersona persona = new PersonaService();
        private IHerramientas herramientas= new HerramientasService();
        public Persona PersonaSelecionada { get; set; }
        private TipoEntidad tipo;
        public VBuscarPersona(TipoEntidad tipo)
        {
            InitializeComponent();
            this.tipo = tipo;
            Cargar();
        }
        public VBuscarPersona()
        {
            InitializeComponent();
            Cargar();
        }
        public async void Cargar()
        {
            data.ItemsSource = await persona.ObtenerPersonas();
        }
        public async void Cargar2()
        {
            data.ItemsSource = await persona.ObtenerPersonas(tipo);
        }
        private void data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (data.SelectedItem is Persona per)
            {
                PersonaSelecionada = per;
                DialogResult = true;
            }
            else
            {
               MessageBox.Show("Seleccione una persona");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var tip = await herramientas.ObtenerEntidades();
            var entidad = tip.Where(x => x.Nombre == "SOCIO")?.FirstOrDefault() ?? null;
            if (entidad == null)
            {
                MessageBox.Show("No existe la entidad SOCIO, por favor cree la entidad SOCIO primero.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            VNewPersona vNewPersona = new VNewPersona(entidad);
            vNewPersona.ShowDialog();
        }
    }
}
