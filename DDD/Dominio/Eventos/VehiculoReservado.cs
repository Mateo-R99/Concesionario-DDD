using System;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Dominio.Eventos
{
    public class VehiculoReservado : DomainEvent
    {
        public Guid VehiculoId { get; }
        public Guid ClienteId { get; }

        public VehiculoReservado(Guid vehiculoId, Guid clienteId)
        {
            VehiculoId = vehiculoId;
            ClienteId = clienteId;
        }
    }
}