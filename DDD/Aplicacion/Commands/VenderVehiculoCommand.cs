using System;

namespace ConcesionarioDDD.Aplicacion.Commands
{
    public class VenderVehiculoCommand
    {
        public Guid VehiculoId { get; set; }

        public VenderVehiculoCommand(Guid vehiculoId)
        {
            VehiculoId = vehiculoId;
        }
    }
}