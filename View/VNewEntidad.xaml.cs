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
    /// Lógica de interacción para VNewEntidad.xaml
    /// </summary>
    public partial class VNewEntidad : Window
    {
        private IHerramientas herramientas = new HerramientasService();
        public TipoEntidad TipoEntidad { get; set; } = new TipoEntidad();
        public VNewEntidad()
        {
            InitializeComponent();
           
            DataContext =TipoEntidad;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
        if( await herramientas.GuardarEntidad(TipoEntidad))
            {
                this.DialogResult = true;
            }else
            {
                this.DialogResult = false;
            }
            
        }
    }
}
