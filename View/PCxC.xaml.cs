using SFCH.IService;
using SFCH.Logica;
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
    /// Lógica de interacción para PCxC.xaml
    /// </summary>
    public partial class PCxC : Page
    {
        IContabilidad contabilidad = new ContabilidadService();
        public PCxC()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
        }
        public async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var cxcs = await contabilidad.OptenerCxCs();
            data.ItemsSource = cxcs;
        }
    }
}
