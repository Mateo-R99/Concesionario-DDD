using System;

namespace ConcesionarioDDD.Aplicacion.Commands
{
    public class ReservarVehiculoCommand
    {
        public Guid VehiculoId { get; set; }

        public ReservarVehiculoCommand(Guid vehiculoId)
        {
            VehiculoId = vehiculoId;
        }
    }
}