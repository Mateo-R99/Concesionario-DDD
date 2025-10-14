using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConcesionarioDDD.Infraestructura.Events
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(
            IServiceProvider serviceProvider,
            ILogger<DomainEventDispatcher> logger)
        {
            _serviceProvider = serviceProvider;
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

                var eventType = domainEvent.GetType();
                var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var handlers = scope.ServiceProvider.GetServices(handlerType);

                    foreach (var handler in handlers)
                    {
                        try
                        {
                            await (Task)handlerType
                                .GetMethod("Handle")!
                                .Invoke(handler, new object[] { domainEvent })!;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error al manejar evento {EventType}", eventType.Name);
                        }
                    }
                }
            }
        }
    }
}