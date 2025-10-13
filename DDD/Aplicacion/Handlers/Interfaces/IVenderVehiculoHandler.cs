using System.Threading.Tasks;
using ConcesionarioDDD.Aplicacion.Commands;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Aplicacion.Handlers.Interfaces
{
    public interface IVenderVehiculoHandler
    {
        Task<Result<decimal>> Handle(VenderVehiculoCommand command);
    }
}