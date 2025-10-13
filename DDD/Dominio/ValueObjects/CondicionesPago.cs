namespace ConcesionarioDDD.Dominio.ValueObjects
{
    public record CondicionesPago
    {
        public string Tipo { get; }
        public int? PlazoPagos { get; }
        public decimal? MontoInicial { get; }

        public CondicionesPago(string tipo, int? plazoPagos = null, decimal? montoInicial = null)
        {
            Tipo = tipo;
            PlazoPagos = plazoPagos;
            MontoInicial = montoInicial;

            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrEmpty(Tipo))
                throw new ArgumentException("El tipo de pago no puede estar vacío");

            if (Tipo.ToLower() == "financiado")
            {
                if (!PlazoPagos.HasValue || PlazoPagos <= 0)
                    throw new ArgumentException("Para pago financiado, debe especificar un plazo válido");

                if (!MontoInicial.HasValue || MontoInicial < 0)
                    throw new ArgumentException("Para pago financiado, debe especificar un monto inicial válido");
            }
        }
    }
}