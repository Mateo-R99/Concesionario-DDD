using System;
using System.Threading.Tasks;
using ConcesionarioDDD.Aplicacion.Commands;
using ConcesionarioDDD.Aplicacion.Handlers.Interfaces;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.SharedKernel;
using ConcesionarioDDD.SharedKernel.Interfaces;

namespace ConcesionarioDDD.Aplicacion.Handlers
{
    public class VenderVehiculoHandler : IVenderVehiculoHandler
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDomainEventDispatcher _eventDispatcher;

        public VenderVehiculoHandler(
            IVehiculoRepositorio vehiculoRepositorio,
            IUnitOfWork unitOfWork,
            IDomainEventDispatcher eventDispatcher)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
            _unitOfWork = unitOfWork;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<Result<decimal>> Handle(VenderVehiculoCommand command)
        {
            try
            {
                var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(command.VehiculoId);

                if (vehiculo == null)
                    return Result<decimal>.Fail("Vehículo no encontrado");

                // Lógica de dominio
                var result = vehiculo.MarcarComoVendido();
                if (!result.IsSuccess)
                    return Result<decimal>.Fail(result.Error);

                await _vehiculoRepositorio.ActualizarAsync(vehiculo);

                // Despachar eventos si hay
                if (vehiculo is IDomainEventEmitter eventEmitter)
                {
                    await _eventDispatcher.DispatchAsync(eventEmitter.GetDomainEvents());
                    eventEmitter.ClearDomainEvents();
                }

                await _unitOfWork.SaveChangesAsync();

                return Result<decimal>.Ok(vehiculo.CalcularPrecioTotal());
            }
            catch (InvalidOperationException ex)
            {
                return Result<decimal>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<decimal>.Fail($"Error al vender vehículo: {ex.Message}");
            }
        }
    }
}