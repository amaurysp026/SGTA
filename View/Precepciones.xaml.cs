using SFCH.IService;
using SFCH.Logica;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para Precepciones.xaml
    /// </summary>
    public partial class Precepciones : Page
    {
        IRecepcion recepcion=new RecepcionService();
        public Precepciones()
        {
            InitializeComponent();
            Cargar();
        }
        private async void Cargar()
        {
            var lista = await recepcion.ObtenerRecepcionesLecheAsync();
            data.ItemsSource = lista;
            dap.DisplayDateStart= DateTime.Now.AddDays(-7);
            dap.DisplayDateEnd= DateTime.Now;
        }

        private async void btnnewrecepcion_Click(object sender, RoutedEventArgs e)
        {
         //   MessageBox.Show("En construcción" + dap.SelectedDate.Value.Date + " y " + DateTime.Now.Date);

            if (dap.SelectedDate.HasValue)
            {
                DateTime selectedDate = dap.SelectedDate.Value;
                if (selectedDate.ToShortDateString()!=DateTime.Now.ToShortDateString())
                {
                 var res=   MessageBox.Show("La fecha seleccionada es diferente a la fecha actual desea continuar?","Aviso!",MessageBoxButton.YesNo,MessageBoxImage.Information);
                    if (res==MessageBoxResult.No)
                        {
                            return;
                    }
                }
                if(cbtanda.SelectedItem==null)
                {
                    MessageBox.Show("Debe seleccionar una tanda.");
                    return;
                }
               ;
                var r = new RecepcionLeche();
                r.Tanda = cbtanda.SelectedValuePath.ToString() ?? string.Empty;
                await  recepcion.GuardarRecepcionLecheAsync(r, selectedDate.Date);
                Cargar();
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ninguna fecha.");
            }
        }
    }
}
