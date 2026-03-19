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
    /// Lógica de interacción para PComprasList.xaml
    /// </summary>
    public partial class PComprasList : Page
    {
        ICompra compra = new CompraService();
        public PComprasList()
        {
            InitializeComponent();
            Cargar();
        }
        public async void Cargar()
        {
            var lis = await compra.ObtenerCompras();
            data.ItemsSource = lis.OrderByDescending(x => x.Id);
        }
    }
}
