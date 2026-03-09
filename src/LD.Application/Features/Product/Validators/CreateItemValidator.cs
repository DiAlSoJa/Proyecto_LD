using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Application.Features.Clients.Queries;
using FluentValidation;
using LD.Application.Features.Items.Comands;

namespace LD.Application.Features.Clients.Validators;

public class CreateItemValidator
    : AbstractValidator<CreateProductCommand>
{
    public CreateItemValidator()
    {
        // 🔹 Project
        RuleFor(x => x.ProjectId)
            .NotNull().WithMessage("El proyecto es obligatorio.")
            .GreaterThan(0).WithMessage("El proyecto debe ser válido.");

        // 🔹 PartNumber
        RuleFor(x => x.PartNumber)
            .NotEmpty().WithMessage("El número de parte es obligatorio.")
            .MaximumLength(50).WithMessage("El número de parte no puede exceder 50 caracteres.")
            .Matches("^[A-Z0-9-_.]+$")
            .WithMessage("El número de parte solo puede contener letras mayúsculas, números, guiones, guión bajo o punto.");

        // 🔹 Description
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .MaximumLength(250).WithMessage("La descripción no puede exceder 250 caracteres.");

        // 🔹 Units
        RuleFor(x => x.MinUnitId)
            .NotNull().WithMessage("La unidad mínima es obligatoria.")
            .GreaterThan(0).WithMessage("La unidad mínima debe ser válida.");

        RuleFor(x => x.MediumUnitId)
            .GreaterThan(0)
            .When(x => x.MediumUnitId.HasValue)
            .WithMessage("La unidad media debe ser válida.");

        RuleFor(x => x.MaxUnitId)
            .GreaterThan(0)
            .When(x => x.MaxUnitId.HasValue)
            .WithMessage("La unidad máxima debe ser válida.");


    }
}
