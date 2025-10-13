using System.Collections.Generic;
using System.Threading.Tasks;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Dominio.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IEnumerable<DomainEvent> events);
    }
}