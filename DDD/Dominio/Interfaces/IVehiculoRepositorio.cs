using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConcesionarioDDD.Dominio.Entidades;

namespace ConcesionarioDDD.Dominio.Interfaces
{
    public interface IVehiculoRepositorio
    {
        Task<Vehiculo?> ObtenerPorIdAsync(Guid id);
        Task ActualizarAsync(Vehiculo vehiculo);
        Task AgregarAsync(Vehiculo vehiculo);
        Task<IEnumerable<Vehiculo>> ObtenerTodosAsync();
    }
}