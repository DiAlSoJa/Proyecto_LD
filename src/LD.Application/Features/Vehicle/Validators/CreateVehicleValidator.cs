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
             .NotEmpty().WithMessage("Las Placas no debe estar vacia")
             .MaximumLength(20).WithMessage("Las Placas del vehículo no debe exceder de 20 caracteres");



    }
}

