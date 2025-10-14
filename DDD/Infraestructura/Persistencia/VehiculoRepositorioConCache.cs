using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Entidades;
using ConcesionarioDDD.Dominio.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcesionarioDDD.Infraestructura.Persistencia
{
    public class VehiculoRepositorioConCache : IVehiculoRepositorio
    {
        private readonly IVehiculoRepositorio _innerRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<VehiculoRepositorioConCache> _logger;
        private const string CACHE_KEY_PREFIX = "vehiculo:";
        private const string CACHE_KEY_ALL = "vehiculos:all";

        public VehiculoRepositorioConCache(
            VehiculoRepositorioEnMemoria innerRepository,
            ICacheService cacheService,
            ILogger<VehiculoRepositorioConCache> logger)
        {
            _innerRepository = innerRepository;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Vehiculo?> ObtenerPorIdAsync(Guid id)
        {
            var cacheKey = $"{CACHE_KEY_PREFIX}{id}";

            // Intentar obtener del caché
            var vehiculoEnCache = await _cacheService.GetAsync<Vehiculo>(cacheKey);
            if (vehiculoEnCache != null)
            {
                _logger.LogInformation("Vehículo {VehiculoId} obtenido del caché", id);
                return vehiculoEnCache;
            }

            // Si no está en caché, obtener del repositorio
            var vehiculo = await _innerRepository.ObtenerPorIdAsync(id);

            if (vehiculo != null)
            {
                // Guardar en caché por 15 minutos
                await _cacheService.SetAsync(cacheKey, vehiculo, TimeSpan.FromMinutes(15));
                _logger.LogInformation("Vehículo {VehiculoId} guardado en caché", id);
            }

            return vehiculo;
        }

        public async Task<IEnumerable<Vehiculo>> ObtenerTodosAsync()
        {
            // Intentar obtener del caché
            var vehiculosEnCache = await _cacheService.GetAsync<IEnumerable<Vehiculo>>(CACHE_KEY_ALL);
            if (vehiculosEnCache != null)
            {
                _logger.LogInformation("Lista de vehículos obtenida del caché");
                return vehiculosEnCache;
            }

            // Si no está en caché, obtener del repositorio
            var vehiculos = await _innerRepository.ObtenerTodosAsync();

            // Guardar en caché por 5 minutos (lista completa cambia más frecuentemente)
            await _cacheService.SetAsync(CACHE_KEY_ALL, vehiculos, TimeSpan.FromMinutes(5));
            _logger.LogInformation("Lista de vehículos guardada en caché");

            return vehiculos;
        }

        public async Task AgregarAsync(Vehiculo vehiculo)
        {
            await _innerRepository.AgregarAsync(vehiculo);

            // Invalidar caché de lista completa
            await _cacheService.RemoveAsync(CACHE_KEY_ALL);

            // Guardar el nuevo vehículo en caché
            var cacheKey = $"{CACHE_KEY_PREFIX}{vehiculo.Id}";
            await _cacheService.SetAsync(cacheKey, vehiculo, TimeSpan.FromMinutes(15));

            _logger.LogInformation("Vehículo {VehiculoId} agregado y caché actualizado", vehiculo.Id);
        }

        public async Task ActualizarAsync(Vehiculo vehiculo)
        {
            await _innerRepository.ActualizarAsync(vehiculo);

            // Invalidar caché del vehículo específico y de la lista completa
            var cacheKey = $"{CACHE_KEY_PREFIX}{vehiculo.Id}";
            await _cacheService.RemoveAsync(cacheKey);
            await _cacheService.RemoveAsync(CACHE_KEY_ALL);

            // Actualizar caché con el vehículo modificado
            await _cacheService.SetAsync(cacheKey, vehiculo, TimeSpan.FromMinutes(15));

            _logger.LogInformation("Vehículo {VehiculoId} actualizado y caché invalidado", vehiculo.Id);
        }
    }
}