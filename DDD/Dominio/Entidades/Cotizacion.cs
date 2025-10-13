using ConcesionarioDDD.SharedKernel;
using ConcesionarioDDD.Dominio.ValueObjects;

namespace ConcesionarioDDD.Dominio.Entidades
{
    public class Cotizacion : Entity<Guid>
    {
        private List<(string Descripcion, Precio Precio)> _extras;

        public Guid VehiculoId { get; private set; }
        public Precio PrecioBase { get; private set; }
        public decimal PorcentajeDescuento { get; private set; }
        public int DiasVigencia { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public DateTime FechaExpiracion { get; private set; }
        public IReadOnlyList<(string Descripcion, Precio Precio)> Extras => _extras.AsReadOnly();
        public Precio PrecioFinal => CalcularPrecioFinal();

        private Cotizacion()
        {
            _extras = new List<(string, Precio)>();
        }

        public Cotizacion(Guid vehiculoId, Precio precioBase, int diasVigencia)
        {
            Id = Guid.NewGuid();
            VehiculoId = vehiculoId;
            PrecioBase = precioBase;
            DiasVigencia = diasVigencia;
            FechaCreacion = DateTime.UtcNow;
            FechaExpiracion = FechaCreacion.AddDays(diasVigencia);
            _extras = new List<(string, Precio)>();
        }

        public void AgregarExtra(string descripcion, Precio precio)
        {
            _extras.Add((descripcion, precio));
        }

        public void AplicarDescuento(decimal porcentaje)
        {
            if (porcentaje < 0 || porcentaje > 100)
                throw new ArgumentException("El porcentaje de descuento debe estar entre 0 y 100");

            PorcentajeDescuento = porcentaje;
        }

        private Precio CalcularPrecioFinal()
        {
            var precioTotal = PrecioBase.Monto;
            foreach (var extra in _extras)
            {
                precioTotal += extra.Precio.Monto;
            }

            if (PorcentajeDescuento > 0)
            {
                var descuento = precioTotal * (PorcentajeDescuento / 100);
                precioTotal -= descuento;
            }

            return new Precio(precioTotal);
        }

        public bool EstaVigente() => DateTime.UtcNow <= FechaExpiracion;
    }
}