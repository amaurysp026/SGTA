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
    /// Lógica de interacción para PTurnos.xaml
    /// </summary>
    public partial class PTurnos : Page, INotifyPropertyChanged
    {

        public Turno turno
        {
            get { return field; }
            set
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(turno)));
            }

        }
        public List<Turno> turnos
        {

            get { return field; }
            set
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(turnos)));

            }
        }
        ITurno turnoS = new TurnoService();

        public event PropertyChangedEventHandler? PropertyChanged;

        public PTurnos()
        {
            InitializeComponent();
            Cargar();
            DataContext = this;

        }
        private async void Cargar()
        {   progreso.Visibility = Visibility.Visible;
            turnos = await turnoS.ObtenerTurnosAsync();
            data.Items.Refresh();
                progreso.Visibility = Visibility.Collapsed;
        }

        private void btnabrirTurno_Click(object sender, RoutedEventArgs e)
        {
            VAbrirTurno vAbrirTurno = new VAbrirTurno();
            vAbrirTurno.ShowDialog();
        }

        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data.SelectedItem != null)
            {
                turno = (Turno)data.SelectedItem;
            }
        }

        private async void btnCerrarTurno_Click(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem != null && MessageBox.Show("Deseas continuar para cerrar el turno?", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            { 
                VCierreCuadre vCierreCuadre = new VCierreCuadre(turno);
                vCierreCuadre.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleccione un turno");


            }
            Cargar();
            //Cargar();
        }

        private async void btnRefrescar_Click(object sender, RoutedEventArgs e)
        {
            Cargar();
        }

        private async void MbtnReimprimirCuad_Click(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem != null)
            {
                var turn = (Turno)data.SelectedItem;
                await turnoS.imprimirCuadreAsync(turno);
            }
            else
            {
                MessageBox.Show("Seleccione un turno");
            }
        }

        private void MbtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            btnCerrarTurno_Click(sender, e);

        }

        private void MbtnVerdetalle_Click(object sender, RoutedEventArgs e)
        {
            if (datadetalle.Visibility == Visibility.Collapsed)
            {
                datadetalle.Visibility = Visibility.Visible;
            }
            else
            {
                datadetalle.Visibility = Visibility.Collapsed;
            }
        }
    }
}
