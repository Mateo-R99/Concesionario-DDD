using System.Collections.Concurrent;
using ConcesionarioDDD.Dominio.Agregados;
using ConcesionarioDDD.Dominio.Interfaces;

namespace ConcesionarioDDD.Infraestructura.Persistencia
{
    public class VehiculoRepositorioEnMemoria : IVehiculoRepositorio
    {
        private readonly ConcurrentDictionary<Guid, VehiculoAgregado> _vehiculos = new();

        public async Task<VehiculoAgregado?> ObtenerPorIdAsync(Guid id)
        {
            return _vehiculos.TryGetValue(id, out var vehiculo) ? vehiculo : null;
        }

        public async Task ActualizarAsync(VehiculoAgregado vehiculo)
        {
            _vehiculos.AddOrUpdate(vehiculo.Id, vehiculo, (_, _) => vehiculo);
        }

        public async Task AgregarAsync(VehiculoAgregado vehiculo)
        {
            _vehiculos.TryAdd(vehiculo.Id, vehiculo);
        }

        public async Task<IEnumerable<VehiculoAgregado>> ObtenerTodosAsync()
        {
            return _vehiculos.Values;
        }

        // Método para agregar algunos datos de prueba
        public void InicializarDatos()
        {
            var vehiculo1 = new VehiculoAgregado("Toyota", "Corolla", 2024, "Rojo", 25000M);
            var vehiculo2 = new VehiculoAgregado("Honda", "Civic", 2024, "Azul", 27000M);
            var vehiculo3 = new VehiculoAgregado("Ford", "Mustang", 2024, "Negro", 45000M);

            _vehiculos.TryAdd(vehiculo1.Id, vehiculo1);
            _vehiculos.TryAdd(vehiculo2.Id, vehiculo2);
            _vehiculos.TryAdd(vehiculo3.Id, vehiculo3);
        }
    }
}