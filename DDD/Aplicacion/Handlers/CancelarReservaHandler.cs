using System;
using System.Threading.Tasks;
using ConcesionarioDDD.Aplicacion.Commands;
using ConcesionarioDDD.Aplicacion.Handlers.Interfaces;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.SharedKernel;
using ConcesionarioDDD.SharedKernel.Interfaces;

namespace ConcesionarioDDD.Aplicacion.Handlers
{
    public class CancelarReservaHandler : ICancelarReservaHandler
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDomainEventDispatcher _eventDispatcher;

        public CancelarReservaHandler(
            IVehiculoRepositorio vehiculoRepositorio,
            IUnitOfWork unitOfWork,
            IDomainEventDispatcher eventDispatcher)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
            _unitOfWork = unitOfWork;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<Result<bool>> Handle(CancelarReservaCommand command)
        {
            try
            {
                var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(command.VehiculoId);

                if (vehiculo == null)
                    return Result<bool>.Fail("Vehículo no encontrado");

                // Ejecutar lógica de dominio
                var result = vehiculo.CancelarReserva();
                if (!result.IsSuccess)
                    return Result<bool>.Fail(result.Error);

                await _vehiculoRepositorio.ActualizarAsync(vehiculo);

                // Despachar eventos
                if (vehiculo is IDomainEventEmitter eventEmitter)
                {
                    await _eventDispatcher.DispatchAsync(eventEmitter.GetDomainEvents());
                    eventEmitter.ClearDomainEvents();
                }

                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<bool>.Fail($"Error al cancelar reserva: {ex.Message}");
            }
        }
    }
}