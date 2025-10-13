namespace ConcesionarioDDD.Aplicacion.DTOs
{
    public class CrearVehiculoDTO
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Año { get; set; }
        public string Color { get; set; }
        public decimal PrecioBase { get; set; }
    }
}