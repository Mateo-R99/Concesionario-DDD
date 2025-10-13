using FluentValidation;
using ConcesionarioDDD.Aplicacion.DTOs;

namespace ConcesionarioDDD.Aplicacion.Validators
{
    public class ModificacionDTOValidator : AbstractValidator<ModificacionDTO>
    {
        public ModificacionDTOValidator()
        {
            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción es obligatoria")
                .Length(5, 200).WithMessage("La descripción debe tener entre 5 y 200 caracteres");

            RuleFor(x => x.Precio)
                .GreaterThanOrEqualTo(0).WithMessage("El precio no puede ser negativo")
                .LessThan(1000000).WithMessage("El precio de la modificación no puede exceder 1,000,000");
        }
    }
}