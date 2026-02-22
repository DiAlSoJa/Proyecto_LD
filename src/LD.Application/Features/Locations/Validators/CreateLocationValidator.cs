using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Application.Features.Clients.Queries;
using FluentValidation;
using LD.Application.Features.Comands;

namespace LD.Application.Features.Clients.Validators;

public class CreateLocationValidator
    : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationValidator()
    {
        RuleFor(x => x.WarehouseCode)
                .NotEmpty().WithMessage("El código del almacén es obligatorio.")
                .MaximumLength(20).WithMessage("El código del almacén no puede exceder 20 caracteres.")
                .Matches("^[A-Z0-9-]+$")
                .WithMessage("El código del almacén solo puede contener letras mayúsculas, números y guiones.");

        RuleFor(x => x.Rack)
            .NotEmpty().WithMessage("El rack es obligatorio.")
            .MaximumLength(20).WithMessage("El rack no puede exceder 20 caracteres.");

        RuleFor(x => x.Aisle)
            .MaximumLength(20).WithMessage("El pasillo no puede exceder 20 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Aisle));

        RuleFor(x => x.Level)
            .MaximumLength(10).WithMessage("El nivel no puede exceder 10 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Level));

        RuleFor(x => x.LocationCode)
            .MaximumLength(30).WithMessage("El código de ubicación no puede exceder 30 caracteres.")
            .Matches("^[A-Z0-9-]*$")
            .WithMessage("El código de ubicación solo puede contener letras mayúsculas, números y guiones.")
            .When(x => !string.IsNullOrWhiteSpace(x.LocationCode));

        RuleFor(x => x.Dimension)
            .MaximumLength(50).WithMessage("La dimensión no puede exceder 50 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Dimension));
    }
}
