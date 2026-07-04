using FluentValidation;
using LD.Application.Features.Driver.Commands;

namespace LD.Application.Features.Driver.Validators;

public class CreateDriverValidator : AbstractValidator<CreateDriverCommand>
{
    public CreateDriverValidator()
    {
        RuleFor(x => x.DriverNumber)
            .NotEmpty().WithMessage("El número de chofer no debe estar vacío")
            .MaximumLength(20).WithMessage("El número de chofer no debe exceder 20 caracteres");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("El nombre completo no debe estar vacío")
            .MaximumLength(150).WithMessage("El nombre completo no debe exceder 150 caracteres");

        RuleFor(x => x.Licence)
            .MaximumLength(50).WithMessage("La licencia no debe exceder 50 caracteres");

        RuleFor(x => x.IMSS)
            .MaximumLength(20).WithMessage("El IMSS no debe exceder 20 caracteres");
    }
}
