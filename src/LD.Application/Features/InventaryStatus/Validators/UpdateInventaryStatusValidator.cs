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

        RuleFor(x => x.FullName)
             .NotEmpty().WithMessage("La descripción del estatus no debe estar vacia")
             .MaximumLength(150).WithMessage("La descripción del estatus no debe exceder de 150 caracteres");

        RuleFor(x => x.ClientId)
             .GreaterThan(0).WithMessage("El cliente es obligatorio");

        RuleFor(x => x.ProjectId)
             .GreaterThan(0).WithMessage("El proyecto es obligatorio");

    }
}


