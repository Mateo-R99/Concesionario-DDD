using ConcesionarioDDD.SharedKernel;
using ConcesionarioDDD.Dominio.ValueObjects;
using ConcesionarioDDD.Dominio.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConcesionarioDDD.Dominio.Entidades
{
    public class Vehiculo : Entity<Guid>
    {
        public string Marca { get; private set; } = string.Empty;
        public string Modelo { get; private set; } = string.Empty;
        public int Año { get; private set; }
        public string Color { get; private set; } = string.Empty;
        public decimal PrecioBase { get; private set; }
        public EstadoVehiculo Estado { get; private set; }

        private readonly List<Modificacion> _modificaciones = new();
        public IReadOnlyCollection<Modificacion> Modificaciones => _modificaciones.AsReadOnly();

        // Propiedad calculada para el precio total
        public decimal PrecioTotal => PrecioBase + _modificaciones.Sum(m => m.Precio);

        private Vehiculo() { } // Para EF Core

        public Vehiculo(string marca, string modelo, int año, string color, decimal precioBase)
        {
            if (string.IsNullOrWhiteSpace(marca))
                throw new ArgumentException("La marca no puede estar vacía", nameof(marca));

            if (string.IsNullOrWhiteSpace(modelo))
                throw new ArgumentException("El modelo no puede estar vacío", nameof(modelo));

            if (año < 1900 || año > DateTime.Now.Year + 1)
                throw new ArgumentException("El año no es válido", nameof(año));

            if (precioBase < 0)
                throw new ArgumentException("El precio no puede ser negativo", nameof(precioBase));

            Id = Guid.NewGuid();
            Marca = marca;
            Modelo = modelo;
            Año = año;
            Color = color;
            PrecioBase = precioBase;
            Estado = EstadoVehiculo.Disponible;
        }

        public Result AgregarModificacion(string descripcion, decimal precio)
        {
            if (Estado == EstadoVehiculo.Vendido)
                return Result.Fail("No se pueden agregar modificaciones a un vehículo vendido");

            if (string.IsNullOrWhiteSpace(descripcion))
                return Result.Fail("La descripción no puede estar vacía");

            if (precio <= 0)
                return Result.Fail("El precio de la modificación debe ser mayor a cero");

            var modificacion = new Modificacion(descripcion, precio);
            _modificaciones.Add(modificacion);

            // Publicar evento de dominio
            AgregarEventoDominio(new ModificacionAgregada(Id, descripcion, precio));

            return Result.Success();
        }

        public Result MarcarComoReservado()
        {
            if (Estado != EstadoVehiculo.Disponible)
                return Result.Fail("El vehículo no está disponible para reserva");

            Estado = EstadoVehiculo.Reservado;

            // Publicar evento de dominio
            AgregarEventoDominio(new VehiculoReservado(Id, Marca, Modelo));

            return Result.Success();
        }

        public Result MarcarComoVendido()
        {
            if (Estado != EstadoVehiculo.Reservado)
                return Result.Fail("El vehículo debe estar reservado para marcarlo como vendido");

            Estado = EstadoVehiculo.Vendido;

            // Publicar evento de dominio
            AgregarEventoDominio(new VehiculoVendido(Id, Marca, Modelo, PrecioTotal));

            return Result.Success();
        }

        public Result CancelarReserva()
        {
            if (Estado != EstadoVehiculo.Reservado)
                return Result.Fail("El vehículo no está reservado");

            Estado = EstadoVehiculo.Disponible;

            // Publicar evento de dominio
            AgregarEventoDominio(new ReservaCancelada(Id));

            return Result.Success();
        }
    }
}