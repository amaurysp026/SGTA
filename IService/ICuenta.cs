using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.IService
{
    public interface ICuenta
    {
        Task<bool> GuardarCuenta(Cuenta cuenta);
        Task<bool> EliminarCuenta(int id);
        Task<List<Cuenta>> ObtenerCuentas();
        Task<Cuenta> ObtenerCuentaPorId(int id);
        Task<bool> TrasnIngreso(Transaccion transaccion);
        Task<bool> DesactivarCuenta(Cuenta cuenta);
        Task<bool> AnularTransa(Transaccion transaccion);
        Task<bool> CalcularTotal(Cuenta cuenta);
        Task<bool> ReporteXCDetalle(List<Cuenta> cuentas);
        Task<bool> CalcularTodoTotal();

    }
}
