
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LD.Application.Features.InventaryStatus.Comands;
namespace LD.Application.Features.Status.Validators;

public class CreateInventaryStatusValidator
    : AbstractValidator<CreateInventaryStatusCommand>
{
    public CreateInventaryStatusValidator()
    {
        /*RuleFor(x => x.)
             .NotEmpty().WithMessage("El nombre del estatus no debe estar vacio")
             .MaximumLength(10).WithMessage("El nombre del estatus no debe exceder de 10 caracteres");
        **/


    }
}

