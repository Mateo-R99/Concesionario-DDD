using System;

namespace ConcesionarioDDD.Aplicacion.Commands
{
    public class ReservarVehiculoCommand
    {
        public Guid VehiculoId { get; }
        public Guid ClienteId { get; }

        public ReservarVehiculoCommand(Guid vehiculoId, Guid clienteId)
        {
            VehiculoId = vehiculoId;
            ClienteId = clienteId;
        }
    }
}