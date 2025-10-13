using System.ComponentModel.DataAnnotations;

namespace ConcesionarioDDD.Aplicacion.DTOs
{
    public class ModificacionDTO
    {
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "La descripción debe tener entre 5 y 200 caracteres")]
        public required string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        public decimal Precio { get; set; }
    }
}