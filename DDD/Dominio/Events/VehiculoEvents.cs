using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Dominio.Events
{
    public class VehiculoReservadoEvent : DomainEvent
    {
        public Guid VehiculoId { get; }

        public VehiculoReservadoEvent(Guid vehiculoId)
        {
            VehiculoId = vehiculoId;
        }
    }

    public class VehiculoVendidoEvent : DomainEvent
    {
        public Guid VehiculoId { get; }

        public VehiculoVendidoEvent(Guid vehiculoId)
        {
            VehiculoId = vehiculoId;
        }
    }

    public class VehiculoReservaEventoCancelado : DomainEvent
    {
        public Guid VehiculoId { get; }

        public VehiculoReservaEventoCancelado(Guid vehiculoId)
        {
            VehiculoId = vehiculoId;
        }
    }
}