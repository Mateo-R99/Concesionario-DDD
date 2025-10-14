using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Eventos;
using ConcesionarioDDD.Dominio.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcesionarioDDD.Infraestructura.Events.Handlers
{
    public class ModificacionAgregadaHandler : IEventHandler<ModificacionAgregada>
    {
        private readonly ILogger<ModificacionAgregadaHandler> _logger;

        public ModificacionAgregadaHandler(ILogger<ModificacionAgregadaHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ModificacionAgregada @event)
        {
            _logger.LogInformation("Se agregó modificación al vehículo {@VehiculoId}: {@Descripcion} por {@Precio:C}",
                @event.VehiculoId, @event.Descripcion, @event.Precio);
            return Task.CompletedTask;
        }
    }
}