using FluentValidation;
using LD.Application.Features.TruckType.Commands;

namespace LD.Application.Features.TruckType.Validators;

public class CreateTruckTypeValidator : AbstractValidator<CreateTruckTypeCommand>
{
    public CreateTruckTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del tipo de camion no debe estar vacio")
            .MaximumLength(100).WithMessage("El nombre del tipo de camion no debe superar 100 caracteres");
    }
}
