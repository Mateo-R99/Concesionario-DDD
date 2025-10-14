using System;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Dominio.Eventos
{
    public class ReservaCancelada : DomainEvent
    {
        public Guid VehiculoId { get; }

        public ReservaCancelada(Guid vehiculoId)
        {
            VehiculoId = vehiculoId;
        }
    }
}