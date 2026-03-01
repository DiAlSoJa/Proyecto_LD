using FluentValidation;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Comands;
using LD.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Clients.Validators;

public class CreateLocationValidator
    : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationValidator()
    {
        RuleFor(x => x.WarehouseId)
           .NotNull().WithMessage("El almacén es obligatorio.");

        RuleFor(x => x.LocationName)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100);

        // ===== Dimensiones =====

        RuleFor(x => x.Height)
            .GreaterThan(0)
            .When(x => x.Height.HasValue);

        RuleFor(x => x.Width)
            .GreaterThan(0)
            .When(x => x.Width.HasValue);

        RuleFor(x => x.Depth)
            .GreaterThan(0)
            .When(x => x.Depth.HasValue);

        // ===== SOLO 1 TIPO =====

        RuleFor(x => x)
            .Must(HaveOnlyOneType)
            .WithMessage("Solo puede seleccionar un tipo.");

        // ===== SOLO 1 SUBTIPO =====

        RuleFor(x => x)
            .Must(HaveOnlyOneSubType)
            .WithMessage("Solo puede seleccionar un subtipo.");

        // ===== SOLO 1 TAMAÑO =====

        RuleFor(x => x)
            .Must(HaveOnlyOneSize)
            .WithMessage("Solo puede seleccionar un tamaño.");
    }


    private bool HaveOnlyOneType(LocationRequest x)
    {
        int count = 0;

        if (x.IsRack) count++;
        if (x.IsCompartidoType) count++;

        return count == 1;
    }


    private bool HaveOnlyOneSubType(LocationRequest x)
    {
        int count = 0;

        if (x.IsGeneral) count++;
        if (x.IsCuarentena) count++;
        if (x.IsEmbarque) count++;
        if (x.IsCompartido) count++;
        if (x.IsReciboYEmbarque) count++;

        return count == 1;
    }


    private bool HaveOnlyOneSize(LocationRequest x)
    {
        int count = 0;

        if (x.IsDoble) count++;
        if (x.IsSencillo) count++;

        return count == 1;
    }

}
