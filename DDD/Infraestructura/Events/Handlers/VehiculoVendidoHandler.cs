using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Eventos;
using ConcesionarioDDD.Dominio.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConcesionarioDDD.Infraestructura.Events.Handlers
{
    public class VehiculoVendidoHandler : IEventHandler<VehiculoVendido>
    {
        private readonly ILogger<VehiculoVendidoHandler> _logger;

        public VehiculoVendidoHandler(ILogger<VehiculoVendidoHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(VehiculoVendido @event)
        {
            _logger.LogInformation("Vehículo {@VehiculoId} {@Marca} {@Modelo} ha sido vendido por {@PrecioFinal:C}",
                @event.VehiculoId, @event.Marca, @event.Modelo, @event.PrecioFinal);
            return Task.CompletedTask;
        }
    }
}
