using System;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Dominio.Events
{
    public class ReservaCancelada : DomainEvent
    {
        public Guid VehiculoId { get; }
        public string Motivo { get; }

        public ReservaCancelada(Guid vehiculoId, string motivo = "Cancelación manual")
        {
            VehiculoId = vehiculoId;
            Motivo = motivo;
        }
    }
}