using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LD.Application.Features.Category.Comands;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Dimensioner.Comands;
using LD.Application.Features.Family.Comands;

namespace LD.Application.Features.Dimensioner.Validators;

public class CreateDimensionerValidator
    : AbstractValidator<CreateDimensionerCommand>
{
    public CreateDimensionerValidator()
    {
        RuleFor(x => x.Description)
             .NotEmpty().WithMessage("La descripción no debe estar vacia")
             .MaximumLength(50).WithMessage("la descripción de la dimensión no debe exceder de 50 caracteres");



    }
}