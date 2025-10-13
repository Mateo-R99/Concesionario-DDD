using System.Threading.Tasks;
using ConcesionarioDDD.Aplicacion.Commands;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Aplicacion.Handlers.Interfaces
{
    public interface IAgregarModificacionHandler
    {
        Task<Result<bool>> Handle(AgregarModificacionCommand command);
    }
}