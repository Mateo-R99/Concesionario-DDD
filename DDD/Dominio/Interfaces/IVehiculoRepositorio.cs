using System;
using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Agregados;

namespace ConcesionarioDDD.Dominio.Interfaces
{
    public interface IVehiculoRepositorio
    {
        Task<VehiculoAgregado?> ObtenerPorIdAsync(Guid id);
        Task ActualizarAsync(VehiculoAgregado vehiculo);
        Task AgregarAsync(VehiculoAgregado vehiculo);
        Task<IEnumerable<VehiculoAgregado>> ObtenerTodosAsync();
    }
}