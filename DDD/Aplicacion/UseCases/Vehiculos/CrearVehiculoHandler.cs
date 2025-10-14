using ConcesionarioDDD.Aplicacion.Commands;
using ConcesionarioDDD.Dominio.Entidades;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Aplicacion.UseCases.Vehiculos
{
    public class CrearVehiculoHandler
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public CrearVehiculoHandler(IVehiculoRepositorio vehiculoRepositorio, IUnitOfWork unitOfWork)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Vehiculo>> Handle(CrearVehiculoCommand command)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var vehiculo = new Vehiculo(
                    command.Marca,
                    command.Modelo,
                    command.Año,
                    command.Color,
                    command.PrecioBase
                );

                await _vehiculoRepositorio.AgregarAsync(vehiculo);
                await _unitOfWork.CommitTransactionAsync();

                return Result<Vehiculo>.Ok(vehiculo);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<Vehiculo>.Fail($"Error al crear el vehículo: {ex.Message}");
            }
        }
    }
}