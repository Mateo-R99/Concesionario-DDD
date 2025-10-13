using System.ComponentModel.DataAnnotations;

namespace ConcesionarioDDD.Aplicacion.DTOs
{
    public class CrearVehiculoDTO
    {
        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "La marca debe tener entre 2 y 50 caracteres")]
        public required string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El modelo debe tener entre 2 y 50 caracteres")]
        public required string Modelo { get; set; }

        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(1900, 2026, ErrorMessage = "El año debe estar entre 1900 y 2026")]
        public int Año { get; set; }

        [Required(ErrorMessage = "El color es obligatorio")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "El color debe tener entre 2 y 30 caracteres")]
        public required string Color { get; set; }

        [Required(ErrorMessage = "El precio base es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioBase { get; set; }
    }
}