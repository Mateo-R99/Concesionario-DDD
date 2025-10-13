namespace ConcesionarioDDD.Infraestructura.Caching
{
    public class CacheConfiguration
    {
        public int DefaultExpirationMinutes { get; set; } = 30;
        public int VehiculoExpirationMinutes { get; set; } = 15;
        public int ListaVehiculosExpirationMinutes { get; set; } = 5;
        public long SizeLimit { get; set; } = 1024; // MB
    }
}