using System;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Dominio.Events
{
    public class VehiculoReservado : DomainEvent
    {
        public Guid VehiculoId { get; }
        public string Marca { get; }
        public string Modelo { get; }
        public DateTime FechaReserva { get; }

        public VehiculoReservado(Guid vehiculoId, string marca, string modelo)
        {
            VehiculoId = vehiculoId;
            Marca = marca;
            Modelo = modelo;
            FechaReserva = DateTime.UtcNow;
        }
    }
}