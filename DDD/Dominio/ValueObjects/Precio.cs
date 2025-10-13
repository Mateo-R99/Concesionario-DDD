using ConcesionarioDDD.SharedKernel;
using System;
using System.Collections.Generic;

namespace ConcesionarioDDD.Dominio.ValueObjects
{
    public class Precio : ValueObject
    {
        public decimal MontoBase { get; init; }
        public string Moneda { get; init; } = "COP";
        public decimal PrecioFinal { get; private set; }

        private Precio() // Para EF Core
        { 
            MontoBase = 0;
            Moneda = "COP";
            PrecioFinal = 0;
        }

        public Precio(decimal montoBase, string moneda = "COP")
        {
            if (montoBase <= 0)
                throw new ArgumentException("El precio debe ser mayor a cero", nameof(montoBase));

            if (string.IsNullOrWhiteSpace(moneda))
                throw new ArgumentException("La moneda es obligatoria", nameof(moneda));

            if (moneda != "COP" && moneda != "USD")
                throw new ArgumentException("La moneda debe ser COP o USD", nameof(moneda));

            MontoBase = montoBase;
            Moneda = moneda;
            PrecioFinal = montoBase;
        }

        public Precio AplicarDescuento(decimal porcentajeDescuento)
        {
            if (porcentajeDescuento < 0 || porcentajeDescuento > 100)
                throw new ArgumentException("El descuento debe estar entre 0 y 100", nameof(porcentajeDescuento));

            var descuento = MontoBase * (porcentajeDescuento / 100);
            var nuevoPrecio = MontoBase - descuento;

            return new Precio(MontoBase, Moneda) { PrecioFinal = nuevoPrecio };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return MontoBase;
            yield return Moneda;
            yield return PrecioFinal;
        }
    }
}