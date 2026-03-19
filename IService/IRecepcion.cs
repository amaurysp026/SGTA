using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.IService
{
    public interface IRecepcion
    {
        Task<bool> GuardarRecepcionLecheAsync(RecepcionLeche recepcionLeche,DateTime Fecha);
        Task<List<RecepcionLeche>> ObtenerRecepcionesLecheAsync();
        Task<DetalleRecepcionLeche> GuardarRegistro(DetalleRecepcionLeche detalleRecepcion);
        Task<RecepcionLeche> OptenerRecepcionLechePorIdAsync(int id);
        Task<List<Freezer>> ObtenerFreezersAsync();
        Task<DetalleRecepcionLeche> EditarRegistroAsync(DetalleRecepcionLeche Registro);
        Task<bool> AnularRegistroAsync(DetalleRecepcionLeche recepcionLeche);
        Task<List<DetalleRecepcionLeche>> ObtenerDetallesPorRecepcionAsync(int recepcionId);
        Task<bool> GuardarFreezer(Freezer freezer);


    }
}
