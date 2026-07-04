using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LD.Application.Features.Vehicule.Comands;

namespace LD.Application.Features.Vehicle.Validators;

public class CreateVehicleValidator
    : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleValidator()
    {
        RuleFor(x => x.Plates)
            .NotEmpty().WithMessage("Las placas no deben estar vacías")
            .MaximumLength(50).WithMessage("Las placas no deben exceder 50 caracteres");

        RuleFor(x => x.VehicleNumber)
            .NotEmpty().WithMessage("El número de vehículo no debe estar vacío")
            .MaximumLength(20).WithMessage("El número de vehículo no debe exceder 20 caracteres");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del vehículo no debe estar vacío")
            .MaximumLength(150).WithMessage("El nombre del vehículo no debe exceder 150 caracteres");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("El tipo de vehículo no debe estar vacío")
            .MaximumLength(50).WithMessage("El tipo de vehículo no debe exceder 50 caracteres");

        RuleFor(x => x.Capacity)
            .GreaterThanOrEqualTo(0).WithMessage("La capacidad no puede ser negativa");

        RuleFor(x => x.Long)
            .GreaterThanOrEqualTo(0).WithMessage("El largo no puede ser negativo");

        RuleFor(x => x.Wight)
            .GreaterThanOrEqualTo(0).WithMessage("El ancho no puede ser negativo");

        RuleFor(x => x.Height)
            .GreaterThanOrEqualTo(0).WithMessage("El alto no puede ser negativo");
    }
}

