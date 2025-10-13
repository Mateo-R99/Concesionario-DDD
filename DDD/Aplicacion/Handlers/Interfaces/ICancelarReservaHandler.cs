using System.Threading.Tasks;
using ConcesionarioDDD.Aplicacion.Commands;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Aplicacion.Handlers.Interfaces
{
    public interface ICancelarReservaHandler
    {
        Task<Result<bool>> Handle(CancelarReservaCommand command);
    }
}