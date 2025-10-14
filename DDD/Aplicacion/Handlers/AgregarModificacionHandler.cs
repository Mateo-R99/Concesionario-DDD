using System;
using System.Threading.Tasks;
using ConcesionarioDDD.Aplicacion.Commands;
using ConcesionarioDDD.Aplicacion.Handlers.Interfaces;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.SharedKernel;
using ConcesionarioDDD.SharedKernel.Interfaces;

namespace ConcesionarioDDD.Aplicacion.Handlers
{
    public class AgregarModificacionHandler : IAgregarModificacionHandler
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDomainEventDispatcher _eventDispatcher;

        public AgregarModificacionHandler(
            IVehiculoRepositorio vehiculoRepositorio,
            IUnitOfWork unitOfWork,
            IDomainEventDispatcher eventDispatcher)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
            _unitOfWork = unitOfWork;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<Result<bool>> Handle(AgregarModificacionCommand command)
        {
            try
            {
                var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(command.VehiculoId);

                if (vehiculo == null)
                    return Result<bool>.Fail("Vehículo no encontrado");

                // Lógica de dominio
                var result = vehiculo.AgregarModificacion(command.Descripcion, command.Precio);
                if (!result.IsSuccess)
                    return Result<bool>.Fail(result.Error);

                await _vehiculoRepositorio.ActualizarAsync(vehiculo);

                // Despachar eventos
                await _eventDispatcher.DispatchAsync(vehiculo.DomainEvents);
                vehiculo.ClearDomainEvents();

                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<bool>.Fail($"Error al agregar modificación: {ex.Message}");
            }
        }
    }
}