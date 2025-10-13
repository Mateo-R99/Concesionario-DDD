using System;
using System.Threading.Tasks;
using ConcesionarioDDD.Aplicacion.Commands;
using ConcesionarioDDD.Aplicacion.Handlers.Interfaces;
using ConcesionarioDDD.Dominio.Agregados;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Aplicacion.Handlers
{
    public class CrearVehiculoHandler : ICrearVehiculoHandler
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public CrearVehiculoHandler(
            IVehiculoRepositorio vehiculoRepositorio,
            IUnitOfWork unitOfWork)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CrearVehiculoCommand command)
        {
            try
            {
                // Crear el agregado de vehículo
                var vehiculo = new VehiculoAgregado(
                    command.Marca,
                    command.Modelo,
                    command.Año,
                    command.Color,
                    command.PrecioBase
                );

                // Agregar al repositorio
                await _vehiculoRepositorio.AgregarAsync(vehiculo);

                // Guardar cambios con transacción
                await _unitOfWork.SaveChangesAsync();

                return Result<Guid>.Ok(vehiculo.Id);
            }
            catch (ArgumentException ex)
            {
                return Result<Guid>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<Guid>.Fail($"Error al crear vehículo: {ex.Message}");
            }
        }
    }
}