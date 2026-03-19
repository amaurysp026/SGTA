using SFCH.Controller;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFCH.PrintView
{
    /// <summary>
    /// Lógica de interacción para PPuntoVen.xaml
    /// </summary>
    public partial class PPuntoVen : Page
    {
        public PPuntoVen()
        {
            InitializeComponent();
        }   
        public PPuntoVen(Transaccion transaccion)
        {
            InitializeComponent();
            img.Source = Util.LoadImage(SesionUsuario.Configuracion.Timbrado??SesionUsuario.Configuracion.LogoPrincipal??new byte[0]);
            txtdireccion.Text = SesionUsuario.Configuracion.Direccion;
            txtTelefono.Text="Tel: "+SesionUsuario.Configuracion.Telefono;
            txtRNC.Text = "RNC " + SesionUsuario.Configuracion.RNC;
            txtTitulo.Text=SesionUsuario.Configuracion.NombreComercial;
            txtfecha1.Text=transaccion.Fecha.ToString("dd/MM/yyyy");
            txtCliente.Text=transaccion.Cliente;
            txtConcep.Text=transaccion.Descripcion;
            txtMonto.Text=transaccion.MontoCredito.ToString("N2");
            txtTotal.Text=transaccion.Monto.ToString("N2");
            txtnotranssac.Text= "Transacción No. " + transaccion.Id;
            txtCedula.Text= transaccion.Cuenta.Titular.Identificacion;
            txtCliente.Text = transaccion.Cuenta.Titular.Nombre;
            txtCuenta.Text =  transaccion.Cuenta.NumeroCuenta;
            txtUsuario.Text = SesionUsuario.Usuario.NombreCompleto;
            try
            {
               //foreach (var item in Util.CargarImpresoras())
               //{
               //    MessageBox.Show(item);
               //}
                PrintDialog printDialog = new PrintDialog();
                string nombreImpresora = SesionUsuario.ConfiguracionLocal.PrintFactura; // Nombre de la impresora específica
                
                        var printServer = new System.Printing.LocalPrintServer();
                        var printers = printServer.GetPrintQueues();
                        var selectedPrinter = printers.FirstOrDefault(p => p.FullName.Contains(SesionUsuario.ConfiguracionLocal.PrintFactura));
                
                        if (selectedPrinter != null)
                        {
                            printDialog.PrintQueue = selectedPrinter;
                            printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Portrait;
                            printDialog.PrintVisual(data, "Volante");
                        }
                        else
                        {
                            throw new Exception($"No se encontró la impresora: {SesionUsuario.ConfiguracionLocal.PrintFactura}");
                        }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al imprimir: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
