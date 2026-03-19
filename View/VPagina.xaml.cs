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
using System.Windows.Shapes;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para VPagina.xaml
    /// </summary>
    public partial class VPagina : Window
    {
        public VPagina( Page page)
        {
            InitializeComponent();
            frame.Content = page;
        }
    }
}
