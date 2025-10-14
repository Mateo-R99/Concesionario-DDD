using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Eventos;
using ConcesionarioDDD.Dominio.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcesionarioDDD.Infraestructura.Events.Handlers
{
    public class VehiculoReservadoHandler : IEventHandler<VehiculoReservado>
    {
        private readonly ILogger<VehiculoReservadoHandler> _logger;

        public VehiculoReservadoHandler(ILogger<VehiculoReservadoHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(VehiculoReservado @event)
        {
            _logger.LogInformation("Vehículo {@VehiculoId} ha sido reservado por cliente {@ClienteId}",
                @event.VehiculoId, @event.ClienteId);
            return Task.CompletedTask;
        }
    }
}
