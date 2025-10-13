using System.Collections.Generic;

namespace ConcesionarioDDD.SharedKernel
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<DomainEvent> EventosDominio { get; }
        void LimpiarEventos();
    }
}