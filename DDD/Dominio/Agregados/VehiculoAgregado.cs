using ConcesionarioDDD.SharedKernel;
using ConcesionarioDDD.Dominio.ValueObjects;
using ConcesionarioDDD.Dominio.Events;

namespace ConcesionarioDDD.Dominio.Agregados
{
    public class VehiculoAgregado : Entity<Guid>
    {
        private readonly List<Modificacion> _modificaciones;
        
        public string Marca { get; private set; }
        public string Modelo { get; private set; }
        public int Año { get; private set; }
        public string Color { get; private set; }
        public Precio PrecioBase { get; private set; }
        public EstadoVehiculo Estado { get; private set; }
        public IReadOnlyCollection<Modificacion> Modificaciones => _modificaciones.AsReadOnly();

        private VehiculoAgregado() 
        {
            _modificaciones = new List<Modificacion>();
            Marca = string.Empty;
            Modelo = string.Empty;
            Color = string.Empty;
            PrecioBase = new Precio(0);  // Valor por defecto para EF Core
        }

        public VehiculoAgregado(string marca, string modelo, int año, string color, decimal precioBase)
        {
            if (string.IsNullOrWhiteSpace(marca))
                throw new ArgumentException("La marca no puede estar vacía", nameof(marca));
            if (string.IsNullOrWhiteSpace(modelo))
                throw new ArgumentException("El modelo no puede estar vacío", nameof(modelo));
            if (año < 1900 || año > DateTime.Now.Year + 1)
                throw new ArgumentException("El año debe ser válido", nameof(año));
            if (string.IsNullOrWhiteSpace(color))
                throw new ArgumentException("El color no puede estar vacío", nameof(color));
            if (precioBase <= 0)
                throw new ArgumentException("El precio base debe ser mayor a cero", nameof(precioBase));

            Id = Guid.NewGuid();
            Marca = marca;
            Modelo = modelo;
            Año = año;
            Color = color;
            PrecioBase = new Precio(precioBase);
            Estado = EstadoVehiculo.Disponible;
            _modificaciones = new List<Modificacion>();
        }

        public Result AgregarModificacion(string descripcion, decimal precio)
        {
            if (Estado == EstadoVehiculo.Vendido)
                return Result.Fail("No se pueden agregar modificaciones a un vehículo vendido");

            if (string.IsNullOrWhiteSpace(descripcion))
                return Result.Fail("La descripción de la modificación no puede estar vacía");

            if (precio <= 0)
                return Result.Fail("El precio de la modificación debe ser mayor a cero");

            var modificacion = new Modificacion(descripcion, precio);
            _modificaciones.Add(modificacion);

            return Result.Success();
        }

        public Result Reservar()
        {
            if (Estado != EstadoVehiculo.Disponible)
                return Result.Fail("El vehículo no está disponible para reserva");

            Estado = EstadoVehiculo.Reservado;
            AddDomainEvent(new VehiculoReservadoEvent(Id));

            return Result.Success();
        }

        public Result MarcarComoVendido()
        {
            if (Estado != EstadoVehiculo.Reservado)
                return Result.Fail("El vehículo debe estar reservado para marcarlo como vendido");

            Estado = EstadoVehiculo.Vendido;
            AddDomainEvent(new VehiculoVendidoEvent(Id));

            return Result.Success();
        }

        public Result CancelarReserva()
        {
            if (Estado != EstadoVehiculo.Reservado)
                return Result.Fail("El vehículo no está reservado");

            Estado = EstadoVehiculo.Disponible;
            AddDomainEvent(new VehiculoReservaEventoCancelado(Id));

            return Result.Success();
        }

        public decimal CalcularPrecioTotal()
        {
            decimal precioModificaciones = _modificaciones.Sum(m => m.Precio);
            return PrecioBase.MontoBase + precioModificaciones;
        }
    }
}