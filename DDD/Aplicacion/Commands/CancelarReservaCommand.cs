namespace ConcesionarioDDD.Aplicacion.Commands
{
    public class CancelarReservaCommand
    {
        public Guid VehiculoId { get; }

        public CancelarReservaCommand(Guid vehiculoId)
        {
            VehiculoId = vehiculoId;
        }
    }
}