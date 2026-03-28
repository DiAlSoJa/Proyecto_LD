using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Application.Features.Clients.Queries;
using FluentValidation;
using LD.Application.Features.Dimensioner.Comands;

namespace LD.Application.Features.Dimensioner.Validators;

public class UpdateDimensionerValidator
    : AbstractValidator<UpdateDimensionerCommand>
{
    public UpdateDimensionerValidator()
    {
        RuleFor(x => x.Description)
             .NotEmpty().WithMessage("La descripción no debe estar vacia")
             .MaximumLength(50).WithMessage("La descripción no debe exceder de 50 caracteres");

    }
}
