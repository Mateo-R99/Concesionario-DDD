using System;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Dominio.Events
{
    public class VehiculoVendido : DomainEvent
    {
        public Guid VehiculoId { get; }
        public string Marca { get; }
        public string Modelo { get; }
        public decimal PrecioFinal { get; }

        public VehiculoVendido(Guid vehiculoId, string marca, string modelo, decimal precioFinal)
        {
            VehiculoId = vehiculoId;
            Marca = marca;
            Modelo = modelo;
            PrecioFinal = precioFinal;
        }
    }
}