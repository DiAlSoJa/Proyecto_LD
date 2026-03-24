using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Application.Features.Clients.Queries;
using FluentValidation;
using LD.Application.Features.Family.Comands;

namespace LD.Application.Features.Family.Validators;

public class UpdateFamilyValidator
    : AbstractValidator<UpdateFamilyCommand>
{
    public UpdateFamilyValidator()
    {
        RuleFor(x => x.FamilyName)
             .NotEmpty().WithMessage("El nombre de la familia no debe estar vacia")
             .MaximumLength(50).WithMessage("El nombre de la familia no debe exceder de 50 caracteres");

    }
}
