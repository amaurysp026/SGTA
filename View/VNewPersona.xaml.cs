using SFCH.IService;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Lógica de interacción para VNewPersona.xaml
    /// </summary>
    public partial class VNewPersona : Window,INotifyPropertyChanged
    {
        private IPersona personaService = new Logica.PersonaService();
        private IHerramientas herramientas = new Logica.HerramientasService();
        private bool editando { get; set; } = false;
        public event PropertyChangedEventHandler? PropertyChanged;
        private Persona _persona = new Persona();
     
        public Persona persona {
            get { return _persona; }
            set
            {
                _persona = value;
                OnPropertyChanged(nameof(persona));
            }
        }
        
        public VNewPersona(TipoEntidad tipoEntidad)
        {
            InitializeComponent();
            txtTitulo.Text = "Registro de Nuevo "+ "Cliente";
            Title= "Registro de Nuevo " + "Cliente";
            persona.TipoEntidad = tipoEntidad;
            cbTIdenti.SelectedIndex = 0;
            this.DataContext = persona;

        }
        public VNewPersona(Persona tpersona)
        {
            InitializeComponent();
            editando = true;
            txtTitulo.Text = "Editar Registro " +"Cliente";
            Title = "Editar Registro " + tpersona.TipoEntidad.Nombre;
            persona = tpersona;
            cbTIdenti.SelectedIndex = 0;
            this.DataContext = persona;

        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (editando)
            {
               // MessageBox.Show(persona?.Direccion?.ToString());
                if (await personaService.EditarPersonasAsync(persona))
                {
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No fue posible la edicion");
                    // DialogResult = false;
                }
            }
            else
            {


                if (await personaService.GuardarPersona(persona))
                {
                    DialogResult = true;
                }
                else
                {
                    // DialogResult = false;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                VBuscarAsog vBuscarAsog = new VBuscarAsog(await personaService.ObtenerPersonasASO());
                var res = vBuscarAsog.ShowDialog();
                if (res == true)
                {
                    var p = vBuscarAsog.entiddadselecionada;
                    persona.Nombre = p.NOMBRE.ToUpper();
                   // MessageBox.Show(p.FECNAC.ToString());
                    persona.Identificacion = p.RNC;
                    persona.FechaNacimiento = DateTime.Parse(p.FECNAC.ToString()).Date;
                    persona.CodSocioAsogafar = p.CODIGO;
                    persona.Telefono = p.TELEFONO1;
                    persona.Celular = p.TELEFONO2;
                    persona.Direccion = p.DIRECCION;
                    txtNombre.Text = persona.Nombre;
                    txtTelefono.Text = persona.Telefono;
                    txtCelular.Text = persona.Celular;
                    txtDireccion.Text = persona.Direccion;
                    txtCodigoaso.Text = persona.CodSocioAsogafar;
                    fchaNac.SelectedDate = persona.FechaNacimiento;
                    txtCedula.Text = persona.Identificacion;
                    OnPropertyChanged(nameof(persona));

                }
            }
            catch (Exception)
            {

                return;
            }
        }
    }
}
