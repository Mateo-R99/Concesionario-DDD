using System.Collections.Generic;
using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.SharedKernel;
using Microsoft.Extensions.Logging;

namespace ConcesionarioDDD.Infraestructura.Events
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(ILogger<DomainEventDispatcher> logger)
        {
            _logger = logger;
        }

        public async Task DispatchAsync(IEnumerable<DomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                _logger.LogInformation(
                    "Evento de dominio despachado: {EventType} - {EventId} - {OcurridoEn}",
                    domainEvent.GetType().Name,
                    domainEvent.EventoId,
                    domainEvent.OcurridoEn
                );

                // Aquí se pueden agregar handlers específicos para cada evento
                // Por ahora solo registramos el evento
                await Task.CompletedTask;
            }
        }
    }
}