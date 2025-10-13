namespace ConcesionarioDDD.Aplicacion.DTOs
{
    public class VehiculoDTO
    {
        public Guid Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Año { get; set; }
        public string Color { get; set; }
        public decimal PrecioBase { get; set; }
        public string Estado { get; set; }
    }
}