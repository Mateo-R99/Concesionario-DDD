using System.Threading.Tasks;
using ConcesionarioDDD.Aplicacion.Commands;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Aplicacion.Handlers.Interfaces
{
    public interface IReservarVehiculoHandler
    {
        Task<Result<bool>> Handle(ReservarVehiculoCommand command);
    }
}