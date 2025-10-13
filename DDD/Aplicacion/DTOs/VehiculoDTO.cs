namespace ConcesionarioDDD.Aplicacion.DTOs
{
    public class VehiculoDTO
    {
        public Guid Id { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public int Año { get; set; }
        public required string Color { get; set; }
        public decimal PrecioBase { get; set; }
        public required string Estado { get; set; }
        public required List<ModificacionDTO> Modificaciones { get; set; } = new();
    }
}