namespace ConcesionarioDDD.Aplicacion.Commands
{
    public class CrearVehiculoCommand
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Año { get; set; }
        public string Color { get; set; }
        public decimal PrecioBase { get; set; }

        public CrearVehiculoCommand(string marca, string modelo, int año, string color, decimal precioBase)
        {
            Marca = marca;
            Modelo = modelo;
            Año = año;
            Color = color;
            PrecioBase = precioBase;
        }
    }
}