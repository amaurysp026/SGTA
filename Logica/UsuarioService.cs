using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using SFCH.Controller;
using SFCH.IService;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace SFCH.Logica
{
    public class UsuarioService : IUsuario
    {
        public Task<bool> CambiarClaveAsync(Usuario usuario, string nuevaClave)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DesactivarUsuarioAsync(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EntradaConfiguracionLocal(string NombreUsuario,bool recoUser)
        {
            using (var db=new Conexion())
            {
               await db.Database.BeginTransactionAsync();
                try
                {
                    var con = await db.ConfiguracionLocals.FirstOrDefaultAsync(x => x.NombreEquipo == Environment.MachineName);
                    if (con==null)
                    {
                        var nuew = new ConfiguracionLocal
                        {
                            NombreEquipo = Environment.MachineName,
                            UltimoInicio = DateTime.Now,
                            usuario = await db.Usuarios.FirstOrDefaultAsync(x=>x.Id== SesionUsuario.Usuario.Id)??new Usuario(),
                            UltimoUsuario = NombreUsuario,
                            UsuarioRec=NombreUsuario
                        };
                      await  db.ConfiguracionLocals.AddAsync(nuew);
                      await  db.SaveChangesAsync();
                      await  db.Database.CommitTransactionAsync();
                        return true;
                    }
                    else
                    {
                        
                        con.UltimoUsuario = SesionUsuario.Usuario.NombreUsuario;
                        con.UltimoInicio= DateTime.Now;
                        if (recoUser)
                        {
                            con.RecordarUsuario = true;
                            con.UsuarioRec = SesionUsuario.Usuario.NombreUsuario;
                        }
                        else
                        {
                            con.RecordarUsuario = false ;
                            con.UsuarioRec = SesionUsuario.Usuario.NombreUsuario;
                        }
                        var existentcon=db.ConfiguracionLocals.FirstOrDefault(x=>x.NombreEquipo==Environment.MachineName);
                       // MessageBox.Show(existentcon.UsosU.ToString());
                        if (existentcon != null)
                        {
                            if (existentcon.UsosU ==0)
                            {
                                existentcon.RecordarUsuario = false;
                                existentcon.UsosU = 500;
                            }
                            else
                            {
                         
                                existentcon.RecordarUsuario = con.RecordarUsuario;
                            }
                             
                            existentcon.UsuarioRec=con.UsuarioRec;
                            existentcon.UltimoInicio = DateTime.Now;
                            existentcon.UsosU = existentcon.UsosU - 1;
                           
                          await  db.SaveChangesAsync() ;
                            await db.Database.CommitTransactionAsync();
                        }
                            return true;
                    }
                }
                catch (Exception EX)
                {
                    MessageBox.Show("Error al guardar configuración local: "+EX.Message);
                    await db.Database.RollbackTransactionAsync();
                    return false;
                }
            }
        }

        public Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async  Task<Usuario?> ObtenerUsuarioPorNombreAsync(string nombreUsuario)
        {
            using (var db=new Conexion())
            {
                try
                {
                  return await  db.Usuarios.Where(x=>x.Activo).FirstOrDefaultAsync(x=>x.NombreUsuario==nombreUsuario);
                }
                catch (Exception ex )
                {
                    MessageBox.Show("Error al obtener usuario: "+ex.Message);

                    return null;
                }
            }
        }

        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
           using(var db=new Conexion())
            {
                return await db.Usuarios.Where(x=>x.Activo).OrderDescending().ToListAsync();
            }

        }
        public async Task<bool>ActualizarUsuario(Usuario usuario)
        {
            using (var db = new Conexion())
            {
                db.Usuarios.Update(usuario);
                if (await db.SaveChangesAsync() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public async Task<ConfiguracionLocal> OptenerConfiguracionLocal()
        {
            using (var db=new Conexion())
            {
                var con = await db.ConfiguracionLocals?.FirstOrDefaultAsync(x => x.NombreEquipo == Environment.MachineName)??new ConfiguracionLocal();
                return con;
            }
        }

        public async Task<bool> RegistrarUsuarioAsync(Usuario usuario)
        {
            if (usuario==null)
            {
                MessageBox.Show("el Usuario es Nulo");
                return false;
            }
            using (var db = new Conexion()) {
              await  db.Database.BeginTransactionAsync();
                try
                {
                    await db.Usuarios.AddAsync(usuario);
                    if (await db.SaveChangesAsync()>0)
                    {
                     await   db.Database.CommitTransactionAsync();
                        return true;
                    }
                  await  db.Database.RollbackTransactionAsync();
                    return false;
                }
                catch (Exception)
                {
                    await db.Database.RollbackTransactionAsync();
                    MessageBox.Show("Error no controlado");
                    return false;
                }
            }
        }

        public async Task<bool> ValidarUsuarioAsync(Usuario usuario, string clave)
        {
          //  using (var db= new Conexion())
       //     {
       //        var d= db.Usuarios.Find(usuario.Id);
          //      d.ContrasenaHash= Util.HashPassword("2626");
         //       await db.SaveChangesAsync();
        //    }
            if ( Util.VerifyPassword(clave, usuario.ContrasenaHash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
