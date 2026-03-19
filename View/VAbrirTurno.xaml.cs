using SFCH.Controller;
using SFCH.IService;
using SFCH.Logica;
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
    /// Lógica de interacción para VAbrirTurno.xaml
    /// </summary>
    public partial class VAbrirTurno : Window
    {
        ITurno turno = new TurnoService();
        public VAbrirTurno()
        {
            InitializeComponent();
        }

        private async void BtnAbrirTurno_Click(object sender, RoutedEventArgs e)
        {
            if (Util.VerifyPassword(txtClave.Password, SesionUsuario.Usuario.ContrasenaHash))
            {
                if (decimal.TryParse(txtMontoInicial.Text, out decimal inical)&&inical>=0)
                {
                    if (await turno.IniciarTurnoAsync(inical))
                    {
                        MessageBox.Show("Turno iniciado con éxito", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error al iniciar el turno", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Monto inicial inválido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                return;
            }
            else
            {
                MessageBox.Show("Clave incorrecta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtMontoInicial_KeyDown(object sender, KeyEventArgs e)
        {
                if (e.Key == Key.Enter)
                {
                    txtClave.Focus();
            }

        }

        private void txtClave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnAbrirTurno_Click(sender, e);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}