using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Dominio.ValueObjects
{
    public class Modificacion : ValueObject
    {
        public string Descripcion { get; private set; } = string.Empty;
        public decimal Precio { get; private set; }

        private Modificacion()
        {
            Descripcion = string.Empty;
            Precio = 0;
        } // Para EF Core

        public Modificacion(string descripcion, decimal precio)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción no puede estar vacía");
            if (precio < 0)
                throw new ArgumentException("El precio no puede ser negativo");

            Descripcion = descripcion;
            Precio = precio;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Descripcion;
            yield return Precio;
        }
    }
}