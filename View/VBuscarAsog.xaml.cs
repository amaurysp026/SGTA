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
    /// Lógica de interacción para VBuscarAsog.xaml
    /// </summary>
    public partial class VBuscarAsog : Window
    {
        public EntidadesASO entiddadselecionada { get; set; }
        public VBuscarAsog(List<EntidadesASO> entidades)
        {
            InitializeComponent();
            data.ItemsSource = entidades;
        }

        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (data.SelectedItem is EntidadesASO enti)
            {
                entiddadselecionada = enti;
                DialogResult = true;
            }
        }
    }
}
