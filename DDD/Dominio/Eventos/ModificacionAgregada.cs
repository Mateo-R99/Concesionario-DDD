using System;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Dominio.Eventos
{
    public class ModificacionAgregada : DomainEvent
    {
        public Guid VehiculoId { get; }
        public string Descripcion { get; }
        public decimal Precio { get; }

        public ModificacionAgregada(Guid vehiculoId, string descripcion, decimal precio)
        {
            VehiculoId = vehiculoId;
            Descripcion = descripcion;
            Precio = precio;
        }
    }
}