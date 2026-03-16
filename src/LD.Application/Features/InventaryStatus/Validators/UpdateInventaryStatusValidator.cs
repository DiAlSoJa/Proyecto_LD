using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Application.Features.Clients.Queries;
using FluentValidation;
using LD.Application.Features.InventaryStatus.Comands;

namespace LD.Application.Features.Status.Validators;

public class UpdateInventaryStatusValidator
    : AbstractValidator<UpdateInventaryStatusCommand>
{
    public UpdateInventaryStatusValidator()
    {
        RuleFor(x => x.InventoryStatusIdS)
             .NotEmpty().WithMessage("El nombre del estatus no debe estar vacio")
             .MaximumLength(20).WithMessage("El nombre del estatus no debe exceder de 20 caracteres");

    }
}


