using Microsoft.EntityFrameworkCore;
using SFCH.Controller;
using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
using SFCH.View;
using SGTA.Model;
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

namespace SGTA.View
{
    /// <summary>
    /// Lógica de interacción para PRecepVehiculo.xaml
    /// </summary>
    public partial class PRecepVehiculo : Page
    {
        public Conexion db = new Conexion();
        public Vehiculo vh { get; set; }
        public List<Vehiculo> vehiculos { get; set; }
        public List<MantVehiculo> mantVehiculos { get; set; }
        IFactura factura= new FacturaService();
        public PRecepVehiculo()
        {
            InitializeComponent();
            cargar();
            vh =new Vehiculo();

            DataContext = this;
        }
        public async void cargar()
        {

            vehiculos = await db.Vehiculos.Where(x=>x.EnTaller==true).ToListAsync();
            //var mat=await db.MantVehiculos.ToListAsync();




            data.ItemsSource = vehiculos;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ent = db.TipoEntidades.FirstOrDefault();
                VNewPersona vNewPersona = new VNewPersona(ent);
                vNewPersona.ShowDialog();
                cargar();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error :" + ex.Message);
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                vh.Persona.TipoEntidad= await db.TipoEntidades.FirstOrDefaultAsync(x=>x.Id==vh.Persona.TipoEntidad.Id);
                if (vh.Persona == null)
                {
                    MessageBox.Show("Es necesario seleccionar un propietario para el vehículo"); return;
                }
                   

                vh.Persona= await db.Personas.FirstOrDefaultAsync(x=>x.Id==vh.Persona.Id);
                await db.Vehiculos.AddAsync(vh);
                if (await db.SaveChangesAsync() > 0)
                {
                    var fa = new Factura { NombreCliente = txtprop.Text, ClienteP = vh.Persona, Abierta = true, Tipo = "Contado", EsContado = true, FechaEmision = DateTime.Now, Total = 0 };
                    var tu = db.Turnos.FirstOrDefault(t => t.Id ==  SesionUsuario.TurnoActual.Id);
                    if (tu != null)
                    {
                        fa.Turno =tu;
                    }
                    else { 
                    MessageBox.Show("Es Necesario un turno abierto");
                        return;
                      }

                    await factura.GuardarFactura(fa,true);
                    MessageBox.Show("Guardado Correctamente");

                    cargar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el registro" + ex.Message, "Error");
                throw;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem is Vehiculo vehiculo)
            {
               var d= db.Find<Vehiculo>(vehiculo.Id);
                d.EnTaller=false;
                db.Update(d);
                db.SaveChanges();
                cargar();
                MessageBox.Show("Vehículo recibido en taller correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Seleccione un vehículo para completar la acción", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnbuscarpro_Click(object sender, RoutedEventArgs e)
        {
            VBuscarPersona vBuscarPersona = new VBuscarPersona();
            vBuscarPersona.ShowDialog();    
            txtprop.Text = vBuscarPersona.PersonaSelecionada.Nombre + " " + vBuscarPersona.PersonaSelecionada.Apellido;
            vh.Persona= vBuscarPersona.PersonaSelecionada;
        }
    }
}
