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
            .MaximumLength(150).WithMessage("El nombre comercial no puede exceder 150 caracteres.");

        RuleFor(x => x.BusinessName)
            .NotEmpty().WithMessage("La razón social es obligatoria.")
            .MaximumLength(150).WithMessage("La razón social no puede exceder 150 caracteres.");

        RuleFor(x => x.Rfc)
            .NotEmpty().WithMessage("El RFC es obligatorio.")
            .Length(12, 13).WithMessage("El RFC debe tener 12 o 13 caracteres.");

        RuleFor(x => x.CommercialAddress)
            .NotEmpty().WithMessage("El domicilio comercial es obligatorio.")
            .MaximumLength(250);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("El teléfono es obligatorio.")
            .Matches(@"^\d{10}$").WithMessage("El teléfono debe tener 10 dígitos.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("La ciudad es obligatoria.")
            .MaximumLength(100);

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("El código postal es obligatorio.")
            .Matches(@"^\d{5}$").WithMessage("El código postal debe tener 5 dígitos.");

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("El estado activo es obligatorio.");
    }
}
