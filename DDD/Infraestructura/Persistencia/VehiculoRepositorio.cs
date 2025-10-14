using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ConcesionarioDDD.Dominio.Entidades;
using ConcesionarioDDD.Dominio.Interfaces;

namespace ConcesionarioDDD.Infraestructura.Persistencia
{
    public class VehiculoRepositorio : IVehiculoRepositorio
    {
        private readonly ConcesionarioDbContext _context;

        public VehiculoRepositorio(ConcesionarioDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vehiculo>> ObtenerTodosAsync()
        {
            return await Task.FromResult(_context.Vehiculos.ToList());
        }

        public async Task<Vehiculo?> ObtenerPorIdAsync(Guid id)
        {
            return await Task.FromResult(_context.Vehiculos.FirstOrDefault(v => v.Id == id));
        }

        public async Task ActualizarAsync(Vehiculo vehiculo)
        {
            var vehiculoExistente = await ObtenerPorIdAsync(vehiculo.Id);
            if (vehiculoExistente == null)
                throw new InvalidOperationException("Vehículo no encontrado");

            _context.Vehiculos.Remove(vehiculoExistente);
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();
        }

        public async Task AgregarAsync(Vehiculo vehiculo)
        {
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();
        }
    }
}