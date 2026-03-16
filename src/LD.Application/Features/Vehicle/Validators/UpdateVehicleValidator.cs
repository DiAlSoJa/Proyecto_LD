
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Units.Comands;
using LD.Application.Features.Vehicle.Comands;

namespace LD.Application.Features.Vehicle.Validators;

public class UpdateVehicleValidator
    : AbstractValidator<UpdateVehicleCommand>
{
    public UpdateVehicleValidator()
    {
        RuleFor(x => x.Plates)
             .NotEmpty().WithMessage("Las Placas no deben de estar vacias")
             .MaximumLength(20).WithMessage("Las Placas no debe exceder de 20 caracteres");

    }
}


