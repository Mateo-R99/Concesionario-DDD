using ConcesionarioDDD.SharedKernel;
using ConcesionarioDDD.Dominio.Events;
using ConcesionarioDDD.Dominio.Interfaces;

namespace ConcesionarioDDD.Dominio.EventHandlers
{
    public interface IEventHandler<TEvent> where TEvent : DomainEvent
    {
        Task Handle(TEvent @event);
    }
}

namespace ConcesionarioDDD.Dominio.EventHandlers
{
    public class VehiculoReservadoHandler : IEventHandler<VehiculoReservadoEvent>
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;

        public VehiculoReservadoHandler(IVehiculoRepositorio vehiculoRepositorio)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
        }

        public async Task Handle(VehiculoReservadoEvent @event)
        {
            // Aquí podrías implementar la lógica cuando un vehículo es reservado
            // Por ejemplo: notificar a otros sistemas, actualizar estadísticas, etc.
            await Task.CompletedTask;
        }
    }
}