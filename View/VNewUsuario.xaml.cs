using SFCH.Controller;
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
    /// Lógica de interacción para VNewUsuario.xaml
    /// </summary>
    public partial class VNewUsuario : Window
    {
        private IUsuario usuarios = new UsuarioService();
        public VNewUsuario()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreCompleto.Text)|| string.IsNullOrWhiteSpace(txtNombreUsuario.Text)|| string.IsNullOrWhiteSpace(txtclave.Password))
            {
                MessageBox.Show("Campos obliatorios No pueden estar en blanco");
                return;
            }
            Usuario usuario = new Usuario()
            {
                NombreCompleto = txtNombreCompleto.Text,
                NombreUsuario = txtNombreUsuario.Text
                ,
                Telefono = txtTelefono.Text,
                ContrasenaHash = Util.HashPassword(txtclave.Password)

            };
           if(await usuarios.RegistrarUsuarioAsync(usuario))
            {
                this.Close();
            }
            ;
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
