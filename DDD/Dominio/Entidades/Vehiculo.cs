using ConcesionarioDDD.SharedKernel;
using ConcesionarioDDD.Dominio.ValueObjects;
using System.Collections.Generic;

namespace ConcesionarioDDD.Dominio.Entidades
{
    public class Vehiculo : Entity<Guid>
    {
        public string Marca { get; private set; }
        public string Modelo { get; private set; }
        public int Año { get; private set; }
        public string Color { get; private set; }
        public decimal PrecioBase { get; private set; }
        public EstadoVehiculo Estado { get; private set; }
        private List<Modificacion> _modificaciones;
        public IReadOnlyCollection<Modificacion> Modificaciones => _modificaciones.AsReadOnly();

        private Vehiculo() { } // Para EF Core

        public Vehiculo(string marca, string modelo, int año, string color, decimal precioBase)
        {
            if (precioBase < 0)
                throw new ArgumentException("El precio no puede ser negativo");

            Id = Guid.NewGuid();
            Marca = marca;
            Modelo = modelo;
            Año = año;
            Color = color;
            PrecioBase = precioBase;
            Estado = EstadoVehiculo.Disponible;
            _modificaciones = new List<Modificacion>();
        }

        public void AgregarModificacion(string descripcion, decimal precio)
        {
            if (Estado == EstadoVehiculo.Vendido)
                throw new InvalidOperationException("No se pueden agregar modificaciones a un vehículo vendido");

            var modificacion = new Modificacion(descripcion, precio);
            _modificaciones.Add(modificacion);
        }

        public void MarcarComoReservado()
        {
            if (Estado != EstadoVehiculo.Disponible)
                throw new InvalidOperationException("El vehículo no está disponible para reserva");

            Estado = EstadoVehiculo.Reservado;
        }

        public void MarcarComoVendido()
        {
            if (Estado != EstadoVehiculo.Reservado)
                throw new InvalidOperationException("El vehículo debe estar reservado para marcarlo como vendido");

            Estado = EstadoVehiculo.Vendido;
        }

        public void CancelarReserva()
        {
            if (Estado != EstadoVehiculo.Reservado)
                throw new InvalidOperationException("El vehículo no está reservado");

            Estado = EstadoVehiculo.Disponible;
        }
    }
}