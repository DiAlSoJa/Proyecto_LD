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
                   .NotEmpty().WithMessage("El nombre comercial es obligatorio.")
                   .MaximumLength(150);

        RuleFor(x => x.CommercialAddress)
            .NotEmpty().WithMessage("El domicilio comercial es obligatorio.")
            .MaximumLength(250);

        RuleFor(x => x.Neightbourhoud)
            .NotEmpty().WithMessage("La colonia es obligatoria.")
            .MaximumLength(100);

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("La ciudad es obligatoria.")
            .MaximumLength(100);

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .Matches(@"^\d{5}$")
            .WithMessage("El código postal debe tener 5 dígitos.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Matches(@"^\d{10}$")
            .WithMessage("El teléfono debe tener 10 dígitos.");

        // ✅ solo valida si no es null
        When(x => x.FicalData != null, () =>
        {
            RuleFor(x => x.FicalData!)
                .SetValidator(new FiscalDataValidator());
        });
    }
}

