using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Application.Features.Clients.Queries;
using FluentValidation;
using LD.Application.Features.Status.Comands;

namespace LD.Application.Features.Status.Validators;

public class UpdateStatusValidator
    : AbstractValidator<UpdateStatusCommand>
{
    public UpdateStatusValidator()
    {
        RuleFor(x => x.Clave)
             .NotEmpty().WithMessage("El nombre del estatus no debe estar vacio")
             .MaximumLength(10).WithMessage("El nombre del estatus no debe exceder de 10 caracteres");

    }
}


