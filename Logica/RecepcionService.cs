using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using SFCH.Controller;
using SFCH.IService;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Text;
using System.Windows;
using System.Windows.Navigation;

namespace SFCH.Logica
{
    public class RecepcionService : IRecepcion
    {
        private Conexion db = new Conexion();



        public async Task<bool> AnularRegistroAsync(DetalleRecepcionLeche Registro)
        {
            await db.Database.BeginTransactionAsync();
            try
            {
                var detalle = await db.DetalleRecepcionLeches.FindAsync(Registro.Id);
                if (detalle != null)
                {
                    detalle.Nulo = true;
                    await db.SaveChangesAsync();
                    await db.Database.CommitTransactionAsync();
                    return true;
                }
                else
                {
                    MessageBox.Show("El registro no existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                await db.Database.RollbackTransactionAsync();
                MessageBox.Show("Error al anular el registro: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }



        public async Task<DetalleRecepcionLeche> EditarRegistroAsync(DetalleRecepcionLeche Registro)
        {

               using var db=new Conexion();
            await db.Database.BeginTransactionAsync();
            try
            {
                var detalle = await db.DetalleRecepcionLeches.FindAsync(Registro.Id);
                if (detalle != null)
                {
                    detalle.Litros = Registro.Litros;
                    detalle.PrecioPorLitro = Registro.PrecioPorLitro;
                    detalle.Monto = Registro.Monto;
                    detalle.Freezer = db.Frezzers.Find(Registro?.Freezer?.Id) ?? new Freezer();
                    db.DetalleRecepcionLeches.Update(detalle);
                    await db.SaveChangesAsync();
                    await db.Database.CommitTransactionAsync();
                    return detalle;
                }
                else
                {
                    MessageBox.Show("El registro no existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return new DetalleRecepcionLeche();
                }
            }
            catch (Exception ex)
            {
                await db.Database.RollbackTransactionAsync();
                MessageBox.Show("Error al editar el registro: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

        }

        public async Task<bool> GuardarRecepcionLecheAsync(RecepcionLeche recepcionLeche,DateTime Fecha)
        {
            await db.Database.BeginTransactionAsync();
            try
            {
                recepcionLeche.Fecha = Fecha;
                recepcionLeche.Usuario = db.Usuarios.Find(SesionUsuario.Usuario.Id) ?? new Usuario();
                var hora = recepcionLeche.Fecha.TimeOfDay;
                if (recepcionLeche.Tanda==string.Empty)
                {

                    // Ejemplo: Tandas comunes en lechería
                    if (hora >= new TimeSpan(4, 0, 0) && hora < new TimeSpan(12, 0, 0))
                        recepcionLeche.Tanda = "Mañana";
                    else if (hora >= new TimeSpan(12, 0, 0) && hora < new TimeSpan(20, 0, 0))
                        recepcionLeche.Tanda = "Tarde";
                    else
                        recepcionLeche.Tanda = "Noche";
                }
                    await db.RecepcionLeches.AddAsync(recepcionLeche);

                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
            
                return true;
            }
            catch (Exception ex)
            {
                await db.Database.RollbackTransactionAsync();
                MessageBox.Show("Error al guardar la recepción de leche: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public async void Cantar(string codigo, string nombre, string litro, string f)
        {
            if (codigo.Length > 3)
            {
                codigo = codigo.Insert(2, " ");
            }

            // Crear una instancia del sintetizador de voz
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            //synthesizer.SelectVoice("Microsoft Raul"); // Cambiar a otra voz

            try
            {
                // Texto a convertir en voz
                string texto = "codigo, " + codigo + " , " + nombre + ", " + litro + " Litros , En Freezer-" + f;

                // Configurar propiedades del sintetizador
                synthesizer.Volume = 100; // Volumen (0 a 100)
                synthesizer.Rate = -1;     // Velocidad (-10 a 10)

                // Convertir texto en voz
             synthesizer.Speak(texto);
                //synthesizer.SpeakAsync(texto);

                // También puedes guardar el audio en un archivo si lo necesitas
                //  string rutaArchivo = "voz_salida.wav";
                // synthesizer.SetOutputToWaveFile(rutaArchivo);
                //  synthesizer.SpeakAsync(texto);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error: " + ex.Message);
            }
            finally
            {
                // Liberar recursos
                synthesizer.Dispose();
            }
        }
        public async Task<DetalleRecepcionLeche> GuardarRegistro(DetalleRecepcionLeche detalleRecepcion)
        {
            using (var db = new Conexion())
            {
                await db.Database.BeginTransactionAsync();
                try
                {
                    await db.RecepcionLeches.FindAsync(detalleRecepcion.RecepcionLeche.Id);
                   
                   
                    detalleRecepcion.RecepcionLeche = await db.RecepcionLeches.FindAsync(detalleRecepcion.RecepcionLeche.Id);
                    
                    detalleRecepcion.Proveedor = db.Entidad.Find(detalleRecepcion.Proveedor.Id) ?? new Entidad();
                    if (detalleRecepcion.Freezer != null)
                    {
                        detalleRecepcion.Freezer = db.Frezzers.Find(detalleRecepcion.Freezer.Id) ?? new Freezer();
                    }
               
                    await db.DetalleRecepcionLeches.AddAsync(detalleRecepcion);
                    await db.SaveChangesAsync();
                    await db.Database.CommitTransactionAsync();
                    if (false)
                    {
                        Thread thread = new Thread(() =>
                        {
                            Cantar(detalleRecepcion.Proveedor.Codigo, detalleRecepcion.Proveedor.NombreCompleto, detalleRecepcion.Litros.ToString(), detalleRecepcion?.Freezer?.Numero ?? "");
                        });
                        thread.Start();
                    }
                    return detalleRecepcion;
                }
                catch (Exception ex)
                {
                    await db.Database.RollbackTransactionAsync();
                    MessageBox.Show("Error al guardar el detalle de la recepción de leche: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
            }

        }

        public async Task<List<DetalleRecepcionLeche>> ObtenerDetallesPorRecepcionAsync(int recepcionId)
        {
            await using var db = new Conexion();
            return await db.DetalleRecepcionLeches.Where(x => !x.Nulo)
                .Include(x => x.Proveedor).ThenInclude(x => x.TipoEntidadAso)
                .Include(x => x.Freezer)
                .Where(x => x.RecepcionLeche.Id == recepcionId)
                .ToListAsync();
        }

        public async Task<List<Freezer>> ObtenerFreezersAsync()
        {
            return await db.Frezzers.Include(x=>x.Recepciones).Include(x=>x.Detalle).Where(x => x.Activo).ToListAsync();
        }

        public Task<List<RecepcionLeche>> ObtenerRecepcionesLecheAsync()
        {
            return db.RecepcionLeches.Include(x => x.Detalles.Where(x => x.Nulo == false)).ThenInclude(x => x.Proveedor).Include(x=>x.Usuario).Include(f => f.Detalles).ThenInclude(g => g.Freezer).ToListAsync();
        }

        public async Task<RecepcionLeche> OptenerRecepcionLechePorIdAsync(int id)
        {
            using var db = new Conexion();
            return await db.RecepcionLeches.Include(x => x.Detalles.Where(x=>x.Nulo==false)).ThenInclude(x => x.Proveedor).Include(d => d.Detalles).ThenInclude(D => D.Freezer).FirstOrDefaultAsync(x => x.Id == id) ?? new RecepcionLeche();
        }


        public async Task<bool> GuardarFreezer(Freezer freezer)
        {
          await  db.Database.BeginTransactionAsync();
            try
            {
                if (freezer.Id>0)
                {
                    var fexist = await db.Frezzers.FindAsync(freezer.Id);
                    if (fexist == null)
                    {
                        MessageBox.Show("No se encontro en base de datos");
                        return false;   
                    }
                    fexist.Descripcion=freezer.Descripcion;
                    fexist.Numero=freezer.Numero;
                    fexist.CapacidadTotal=freezer.CapacidadTotal;
                    
                    await db.SaveChangesAsync();
                    await db.Database.CommitTransactionAsync();
                    return true;

                }
                else
                {
                    await db.Frezzers.AddAsync(freezer);
                    await db.SaveChangesAsync();
                    await db.Database.CommitTransactionAsync();
                    return true;
                }
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar Freezer: "+ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                await db.Database.RollbackTransactionAsync();
                return false;
               
            }
        }
    }
}
