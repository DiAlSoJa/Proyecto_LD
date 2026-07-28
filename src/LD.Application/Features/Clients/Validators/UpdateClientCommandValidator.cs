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
                   .MaximumLength(150).WithMessage("El nombre comercial no puede superar 150 caracteres.");

        RuleFor(x => x.CommercialAddress)
            .NotEmpty().WithMessage("El domicilio comercial es obligatorio.")
            .MaximumLength(250).WithMessage("El domicilio comercial no puede superar 250 caracteres.");

        RuleFor(x => x.Neightbourhoud)
            .NotEmpty().WithMessage("La colonia es obligatoria.")
            .MaximumLength(100).WithMessage("La colonia no puede superar 100 caracteres.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("La ciudad es obligatoria.")
            .MaximumLength(100).WithMessage("La ciudad no puede superar 100 caracteres.");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("El código postal es obligatorio.")
            .Matches(@"^\d{5}$")
            .WithMessage("El código postal debe tener 5 dígitos.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("El teléfono es obligatorio.")
            .Matches(@"^\d{10}$")
            .WithMessage("El teléfono debe tener 10 dígitos.");

        // ✅ solo valida si no es null
        When(x => x.FiscalData != null, () =>
        {
            RuleFor(x => x.FiscalData!)
                .SetValidator(new FiscalDataValidator());
        });
    }
}

