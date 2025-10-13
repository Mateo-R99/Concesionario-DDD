using System;
using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Entidades;

namespace ConcesionarioDDD.Dominio.Interfaces
{
    public interface IServicioPostVenta
    {
        Task RegistrarEntregaAsync(Guid ventaId, DateTime fechaEntrega);
        Task ProgramarPrimerMantenimientoAsync(Guid ventaId);
        Task NotificarGarantiaActivadaAsync(Guid ventaId);
    }
}