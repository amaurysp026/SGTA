using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SFCH.IService;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SFCH.Logica
{
    public class PersonaService : IPersona
    {
        public Task<bool> EliminarPersona(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GuardarPersona(Persona persona)
        {
            try
            {
                using (var db=new Conexion())
                {
                    var tip = db.TipoEntidades.Find(persona.TipoEntidad.Id);
                    if (tip == null)
                    {
                        MessageBox.Show("El tipo de entidad no existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(persona.TipoIdentificacion))
                    {
                        MessageBox.Show("No es posible guardar una persona sin tipo de identificación.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(persona.Identificacion))
                    {
                        MessageBox.Show("No es posible guardar una persona sin identificación.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        return false;
                    }
                    if (persona.Identificacion.Length<11)
                    {
                        MessageBox.Show("No es posible guardar una persona, identificación invalida.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        return false;
                    }
                    var existingPersona = await db.Personas.AsNoTracking().FirstOrDefaultAsync(p => p.Identificacion == persona.Identificacion);
                    if (existingPersona!=null)
                    {
                        MessageBox.Show("Ya existe una persona con la misma identificación. "+existingPersona.Identificacion+" "+existingPersona.Nombre+" Ya esta registrado", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        return false;
                    }


                    persona.TipoEntidad = tip;
                   await db.Personas.AddAsync(persona);
             

                    if ( await db.SaveChangesAsync()>0)
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("No se pudo guardar la persona.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la persona: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<Persona> ObtenerPersonaPorCedula(string t)
        {
           using (var db=new Conexion())
            {
                var persona= await db.Personas.Include(x => x.CxCs).Include(x=>x.TipoEntidad).Include(x=>x.Cuentas).ThenInclude(x=>x.TipoCuenta).AsNoTracking().FirstOrDefaultAsync(p => p.Identificacion == t);
                return persona;
            }
        }

        public async Task<Persona> ObtenerPersonaPorId(int id)
        {
            try
            {
                using (var db=new Conexion())
                {
                    var persona= await db.Personas.FindAsync(id);
                    if (persona == null)
                    {
                        throw new Exception("La persona no existe.");
                    }
                    return persona;
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show("Error al obtener la persona: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        public async Task<List<Persona>> ObtenerPersonas()
        {
            try
            {
                using (var db=new Conexion())
                {
                    return await db.Personas.Include(x=>x.TipoEntidad).Include(x=>x.CxCs).OrderDescending().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener las personas: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public async Task<List<Persona>> ObtenerPersonas(TipoEntidad tipoEntidad)
        {
            try
            {
                using (var db = new Conexion())
                {
                    return await db.Personas.Include(x=>x.TipoEntidad).Include(x => x.CxCs).Where(x=>x.TipoEntidad==tipoEntidad).AsNoTracking().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener las personas: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        public async Task<bool> EditarPersonasAsync(Persona newpersona)
        {
            try
            {
                using (var db = new Conexion())
                {
                var r=  await db.Database.BeginTransactionAsync();
                    Persona per = await db.Personas.Include(x => x.TipoEntidad).FirstOrDefaultAsync(x => x.Id == newpersona.Id);
                    if (per == null && per?.TipoEntidad == null)
                    {
                        MessageBox.Show("La persona o la Entidad no existe");
                        return false;
                        
                    }
                 
                    per.Nombre = newpersona.Nombre;
                    per.Apellido= newpersona.Apellido;
                    per.Telefono= newpersona.Telefono;
                    per.Email= newpersona.Email;
                    per.MontoDescuento=newpersona.MontoDescuento;
                    per.Celular=newpersona.Celular;
                    per.Identificacion=newpersona.Identificacion;
                    per.Observacion=newpersona.Observacion;
                    per.CodSocioAsogafar = newpersona.CodSocioAsogafar;
                    per.Direccion=newpersona.Direccion;
                    per.FechaIngreso = newpersona.FechaIngreso;
                    per.FechaNacimiento = newpersona.FechaNacimiento;
                    per.CodSocioAsogafar = newpersona.CodSocioAsogafar;
                    
                   // MessageBox.Show(per.Direccion);
                    db.Update(per);
                    if (await db.SaveChangesAsync() > 0)
                    {
                       await r.CommitAsync();
                        return true;
                        
                    }
                    else
                    {
                     await   r.RollbackAsync();
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<List<EntidadesASO>> ObtenerPersonasASO()
        {
            try
            {
                List<EntidadesASO> list = JsonConvert.DeserializeObject<List<EntidadesASO>>(File.ReadAllText("C:\\SFCH\\Data\\Entidades.json"))??new List<EntidadesASO>();
                return list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener las personas: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

       
    }
}
