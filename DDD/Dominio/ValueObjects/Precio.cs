namespace ConcesionarioDDD.Dominio.ValueObjects
{
    public record Precio
    {
        public decimal Monto { get; }

        public Precio(decimal monto)
        {
            if (monto < 0)
                throw new ArgumentException("El precio no puede ser negativo");

            Monto = monto;
        }
    }
}