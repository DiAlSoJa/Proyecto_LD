
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Application.Features.Clients.Queries;
using FluentValidation;
using LD.Application.Features.Units.Comands;

namespace LD.Application.Features.Clients.Validators;

public class CreateUnitValidator
    : AbstractValidator<CreateUnitCommand>
{
    public CreateUnitValidator()
    {
        RuleFor(x => x.Clave)
             .NotEmpty().WithMessage("El nombre de la unidad no debe estar vacia")
             .MaximumLength(20).WithMessage("El nombre de la unidad no debe exceder de 20 caracteres");



    }
}

