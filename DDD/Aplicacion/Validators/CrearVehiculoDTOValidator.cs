using FluentValidation;
using ConcesionarioDDD.Aplicacion.DTOs;
using System;

namespace ConcesionarioDDD.Aplicacion.Validators
{
    public class CrearVehiculoDTOValidator : AbstractValidator<CrearVehiculoDTO>
    {
        public CrearVehiculoDTOValidator()
        {
            RuleFor(x => x.Marca)
                .NotEmpty().WithMessage("La marca es obligatoria")
                .Length(2, 50).WithMessage("La marca debe tener entre 2 y 50 caracteres")
                .Matches("^[a-zA-Z0-9 ]+$").WithMessage("La marca solo puede contener letras, números y espacios");

            RuleFor(x => x.Modelo)
                .NotEmpty().WithMessage("El modelo es obligatorio")
                .Length(2, 50).WithMessage("El modelo debe tener entre 2 y 50 caracteres");

            RuleFor(x => x.Año)
                .InclusiveBetween(1900, DateTime.Now.Year + 1)
                .WithMessage($"El año debe estar entre 1900 y {DateTime.Now.Year + 1}");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("El color es obligatorio")
                .Length(2, 30).WithMessage("El color debe tener entre 2 y 30 caracteres");

            RuleFor(x => x.PrecioBase)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0")
                .LessThan(10000000).WithMessage("El precio no puede exceder 10,000,000");
        }
    }
}