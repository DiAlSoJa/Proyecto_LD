using FluentValidation;
using LD.Application.Features.User.Commands;

namespace LD.Application.Features.User.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es requerido");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre completo es requerido");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo es requerido")
            .EmailAddress().WithMessage("El correo no tiene un formato válido");

        // La contraseña es opcional al editar.
        // Si viene algo, debe cumplir los requisitos de Identity y coincidir con la confirmación.
        When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
        {
            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres")
                .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una mayúscula")
                .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una minúscula")
                .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número")
                .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Las contraseñas no coinciden");
        });
    }
}
