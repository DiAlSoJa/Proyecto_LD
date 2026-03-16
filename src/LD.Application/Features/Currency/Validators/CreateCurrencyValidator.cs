
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Application.Features.Clients.Queries;
using FluentValidation;
using LD.Application.Features.Currency.Comands;

namespace LD.Application.Features.Currency.Validators;

public class CreateCurrencyValidator
    : AbstractValidator<CreateCurrencyCommand>
{
    public CreateCurrencyValidator()
    {
        RuleFor(x => x.CurrencyIdS)
             .NotEmpty().WithMessage("El nombre de la moneda no debe estar vacia")
             .MaximumLength(5).WithMessage("El nombre de la moneda no debe exceder de 5 caracteres");



    }
}

