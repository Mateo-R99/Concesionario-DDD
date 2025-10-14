using System.Collections.Concurrent;
using ConcesionarioDDD.Dominio.Entidades;
using ConcesionarioDDD.Dominio.Interfaces;

namespace ConcesionarioDDD.Infraestructura.Persistencia
{
    public class VehiculoRepositorioEnMemoria : IVehiculoRepositorio
    {
        private readonly ConcurrentDictionary<Guid, Vehiculo> _vehiculos = new();

        public async Task<Vehiculo?> ObtenerPorIdAsync(Guid id)
        {
            return _vehiculos.TryGetValue(id, out var vehiculo) ? vehiculo : null;
        }

        public async Task ActualizarAsync(Vehiculo vehiculo)
        {
            _vehiculos.AddOrUpdate(vehiculo.Id, vehiculo, (_, _) => vehiculo);
        }

        public async Task AgregarAsync(Vehiculo vehiculo)
        {
            _vehiculos.TryAdd(vehiculo.Id, vehiculo);
        }

        public async Task<IEnumerable<Vehiculo>> ObtenerTodosAsync()
        {
            return _vehiculos.Values;
        }

        // Método para agregar algunos datos de prueba
        public void InicializarDatos()
        {
            var vehiculo1 = new Vehiculo("Toyota", "Corolla", 2024, "Rojo", 25000M);
            var vehiculo2 = new Vehiculo("Honda", "Civic", 2024, "Azul", 27000M);
            var vehiculo3 = new Vehiculo("Ford", "Mustang", 2024, "Negro", 45000M);

            _vehiculos.TryAdd(vehiculo1.Id, vehiculo1);
            _vehiculos.TryAdd(vehiculo2.Id, vehiculo2);
            _vehiculos.TryAdd(vehiculo3.Id, vehiculo3);
        }
    }
}