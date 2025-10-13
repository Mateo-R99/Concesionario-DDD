using System;
using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Entidades;

namespace ConcesionarioDDD.Dominio.Interfaces
{
    public interface ICotizacionRepositorio
    {
        Task<Cotizacion?> ObtenerPorIdAsync(Guid id);
        Task AgregarAsync(Cotizacion cotizacion);
        Task ActualizarAsync(Cotizacion cotizacion);
    }
}