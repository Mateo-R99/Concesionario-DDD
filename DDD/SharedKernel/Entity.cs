using System;
using System.Collections.Generic;

namespace ConcesionarioDDD.SharedKernel
{
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; } = default!;

        private List<DomainEvent> _domainEvents = new();
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}