using System.Collections.Generic;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.SharedKernel.Interfaces
{
    public interface IDomainEventEmitter
    {
        IReadOnlyCollection<DomainEvent> GetDomainEvents();
        void ClearDomainEvents();
        void AddDomainEvent(DomainEvent eventItem);
    }
}