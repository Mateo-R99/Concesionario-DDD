using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Eventos;
using ConcesionarioDDD.Dominio.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcesionarioDDD.Infraestructura.Events.Handlers
{
    public class ReservaCanceladaHandler : IEventHandler<ReservaCancelada>
    {
        private readonly ILogger<ReservaCanceladaHandler> _logger;

        public ReservaCanceladaHandler(ILogger<ReservaCanceladaHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ReservaCancelada @event)
        {
            _logger.LogInformation("La reserva del vehículo {@VehiculoId} ha sido cancelada", @event.VehiculoId);
            return Task.CompletedTask;
        }
    }
}
