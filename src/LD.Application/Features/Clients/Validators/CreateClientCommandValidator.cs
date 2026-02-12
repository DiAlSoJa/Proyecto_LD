using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Application.Features.Clients.Queries;
using FluentValidation;

namespace LD.Application.Features.Clients.Validators;

public class CreateClientCommandValidator
    : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(x => x.CommercialName)
            .NotEmpty()
            .MaximumLength(20);

        //RuleFor(x => x.BusinessName)
        //    .NotEmpty()
        //    .MaximumLength(20);
    }
}
