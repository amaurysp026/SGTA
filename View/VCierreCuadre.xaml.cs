using SFCH.Controller;
using SFCH.IService;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Lógica de interacción para VCierreCuadre.xaml
    /// </summary>
    public partial class VCierreCuadre : Window
    {
        private Turno tturno { get; set; }
        private ITurno turno1 = new Logica.TurnoService();
        public VCierreCuadre(Turno turno ,bool actual=false)
        {
            InitializeComponent();
     
                 Cargar(turno.Id);



        }
        public async void Cargar(int id)
        {
            tturno= await turno1.ObtenerTurnoAsync(id);
            if (tturno.Abierto==false)
            {
                MessageBox.Show("Turno ya esta cerrado");
                this.Close();
            }
            var fact = tturno.Facturas
                   .Where(f => f.Turno.Id == tturno.Id && !f.Anulada);
            tturno.Ventas = fact
                   .Sum(f => f.Total);
            tturno.EfectivoContado =fact
                    .Sum(f => f.Efectivo);
              //  MessageBox.Show("Turno ya esta cerrado"+ tturno.EfectivoContado);

            tturno.Tarjetas = tturno.Facturas
                    .Where(f => f.Turno.Id == tturno.Id && !f.Anulada && f.Condicion == "Tarjeta")
                    .Sum(f => f.Tarjeta);
            tturno.Cheques = tturno.Facturas
                    .Where(f => f.Turno.Id == tturno.Id && !f.Anulada && f.Condicion == "Cheque")
                    .Sum(f => f.Cheque);
            //MessageBox.Show(tturno.TotalFinal.ToString("c"));
            tturno.TotalFinal =  tturno.EfectivoContado + tturno.ContratosTractor.Sum(x => x.TotalPagado) - tturno.DetalleTurnos.Sum(x => x.Egreso) + tturno.DetalleTurnos.Sum(x => x.Ingreso);
            tturno.Diferencia = tturno.DifernciaCalculada;
          //  MessageBox.Show(tturno.TotalFinal.ToString("c"));

            txtVentasTotales.Text = tturno.Ventas.ToString("C");
            txtEfectivoContado.Text = tturno.EfectivoContado.ToString("C");
            txtTotalDeclarado.Text = tturno.Efectivo.ToString("C");
            txtTEgreso.Text = tturno.DetalleTurnos.Sum(x => x.Egreso).ToString("C");
            txtTingreso.Text = tturno.DetalleTurnos.Sum(x => x.Ingreso).ToString("C");
            txtTotalFinal.Text = tturno.TotalFinal.ToString("C");
            txtDiferencia.Text = tturno.Diferencia.ToString("C");
            txtEfInicial.Text = tturno.TotalInicial.ToString("C");
            txtCheques.Text = tturno.Cheques.ToString("C");
            txtVentasCredito.Text = tturno.Facturas.Sum(x => x.TotalPendiente).ToString("C");

            data2.ItemsSource = tturno.DetalleTurnos.Where(x => x.Egreso > 0).ToList();
            skdiferencia.Background = tturno.DifernciaCalculada > 0 ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.IndianRed);
            skEfInicial.Visibility = tturno.TotalInicial > 0 ? Visibility.Visible : Visibility.Collapsed;
            skcheques.Visibility = tturno.Cheques > 0 ? Visibility.Visible : Visibility.Collapsed;
            skEgresos.Visibility = tturno.DetalleTurnos.Sum(x => x.Egreso) > 0 ? Visibility.Visible : Visibility.Collapsed;
            skingresos.Visibility = tturno.DetalleTurnos.Sum(x => x.Ingreso) > 0 ? Visibility.Visible : Visibility.Collapsed;
            skVCredito.Visibility = tturno.Facturas.Any(x => x.TotalPendiente > 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void txtcantidad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtdenominacion.Focus();
            }
        }

        private void txtdenominacion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
                txtdenominacion.Text = "";
                txtcantidad.Text = "";
                txtcantidad.Focus();
            }
        }

        private async void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            tturno.Observaciones = txtobservaciones.Text;
            if (tturno.Diferencia!=0&&tturno.Observaciones.Count()<3)
            {
                MessageBox.Show("Existe una diferencia de efectivo en el cuadre. Especifica en observación la explicación de la diferencia para continuar.", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            btnCerrar.IsEnabled = false;
           var rd= await turno1.CerrarTurnoAsync(tturno);
            if (rd)
                this.Close();


            btnCerrar.IsEnabled = true;
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            if (decimal.TryParse(txtdenominacion.Text, out var denominacion) && int.TryParse(txtcantidad.Text, out var cantidad))
            {
                if (denominacion <= 0 || cantidad <= 0)
                {
                    MessageBox.Show("Ingrese una denominación y cantidad válidas.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var b = new DesgloseBilletes()
                {
                    Denominacion = denominacion,
                    Cantidad = cantidad,
                };
                if (tturno.DesgloseBilletes.Any(x=>x.Denominacion==b.Denominacion))
                {
                 var rs=   MessageBox.Show("Ya se ha ingresado esa denominación. Si desea modificar la cantidad, elimine el registro y vuelva a ingresarlo con la cantidad correcta.(Si para eliminar)", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (rs == MessageBoxResult.Yes) {                      
                        
                        var item = tturno.DesgloseBilletes.FirstOrDefault(x => x.Denominacion == b.Denominacion);
                        if (item != null)
                        {
                            tturno.DesgloseBilletes.Remove(item);
                            data.ItemsSource = tturno.DesgloseBilletes.OrderByDescending(x => x.Denominacion);
                            data.Items.Refresh();
                            return;
                        }
                    }
                    else
                    {
                        tturno.DesgloseBilletes.First(x=>x.Denominacion==b.Denominacion).Cantidad+=b.Cantidad;
                        decimal tota2l = tturno.DesgloseBilletes.Sum(x => x.Total);
                        data.ItemsSource = tturno.DesgloseBilletes.OrderByDescending(x => x.Denominacion);
                        data.Items.Refresh();

                        txtTotalDesglose.Text = "Total : RD" + tota2l.ToString("C");
                        tturno.Efectivo = tota2l;
                        txtTotalDeclarado.Text = tturno.Efectivo.ToString("C");

                        skdiferencia.Background = tturno.DifernciaCalculada == 0 ? new SolidColorBrush(Colors.White) : skdiferencia.Background = tturno.DifernciaCalculada > 0 ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.IndianRed); ;
                        tturno.Diferencia = tturno.DifernciaCalculada;
                        txtDiferencia.Text = tturno.DifernciaCalculada.ToString("C");
                        txtcantidad.Text = "";
                        txtcantidad.Focus();
                        txtdenominacion.Text = "";

                        return;
                    }  
                }
                tturno.DesgloseBilletes.Add(b);
                decimal total = tturno.DesgloseBilletes.Sum(x => x.Total);
                data.ItemsSource = tturno.DesgloseBilletes.OrderByDescending(x => x.Denominacion);
                data.Items.Refresh();

                txtTotalDesglose.Text = "Total : RD" + total.ToString("C");
                tturno.Efectivo = total;
                txtTotalDeclarado.Text = tturno.Efectivo.ToString("C");

                skdiferencia.Background = tturno.DifernciaCalculada == 0 ? new SolidColorBrush(Colors.White) : skdiferencia.Background = tturno.DifernciaCalculada > 0 ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.IndianRed); ;
                tturno.Diferencia = tturno.DifernciaCalculada;
                txtDiferencia.Text = tturno.DifernciaCalculada.ToString("C");
            }
            else
            {
                MessageBox.Show("Ingrese una denominación y cantidad válidas.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnremovedes_Click(object sender, RoutedEventArgs e)
        {
            if(data.SelectedItem is DesgloseBilletes selected)
            {
                tturno.DesgloseBilletes.Remove(selected);
                decimal total = tturno.DesgloseBilletes.Sum(x => x.Total);
                data.ItemsSource = tturno.DesgloseBilletes.OrderByDescending(x => x.Denominacion);
                data.Items.Refresh();
                txtTotalDesglose.Text = "Total : RD" + total.ToString("C");
                tturno.Efectivo = total;
                txtEfectivoContado.Text = tturno.Efectivo.ToString("C");
                skdiferencia.Background = tturno.DifernciaCalculada == 0 ? new SolidColorBrush(Colors.White) : skdiferencia.Background = tturno.DifernciaCalculada > 0 ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.IndianRed); ;
                tturno.Diferencia = tturno.DifernciaCalculada;
                txtDiferencia.Text = tturno.DifernciaCalculada.ToString("C");
            }
        }
    }
}
