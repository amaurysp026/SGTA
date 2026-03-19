using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.IService
{
    public interface ITurno
    {
        Task<bool> IniciarTurnoAsync(decimal MontoInicial);
        Task<bool> CerrarTurnoAsync(Turno turno);
        Task<bool> RegistrarMovimientoCajaAsync(Factura factra);
        Task<decimal> ObtenerSaldoCajaAsync(int usuarioId);
        Task<bool> VerificarTurnoAbiertoAsync(Usuario usuario);
        Task<List<Turno>> ObtenerTurnosAsync();
        Task<Turno> ObtenerTurnoAsync(int id);
        Task<int> FacturasAbiertas(Turno turno);
        Task<List<Factura>>OptenerFacturasAbiertas(Turno turno);
        Task<bool> imprimirCuadreAsync(Turno turno);
        Task<DetalleTurno> RegistrarEgreso(string Nombre, decimal Monto);
    }
}
