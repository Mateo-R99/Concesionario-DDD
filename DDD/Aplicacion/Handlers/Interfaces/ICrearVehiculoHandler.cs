using System.Threading.Tasks;
using ConcesionarioDDD.Aplicacion.Commands;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Aplicacion.Handlers.Interfaces
{
    public interface ICrearVehiculoHandler
    {
        Task<Result<Guid>> Handle(CrearVehiculoCommand command);
    }
}