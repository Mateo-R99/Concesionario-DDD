using System.Threading.Tasks;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Dominio.Interfaces
{
    public interface IEventHandler<in T> where T : DomainEvent
    {
        Task Handle(T @event);
    }
}