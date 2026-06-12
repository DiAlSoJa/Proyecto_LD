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

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("El correo no tiene un formato valido");
        });

        // La contrasena es opcional al editar.
        // Si viene algo, debe cumplir los requisitos de Identity y coincidir con la confirmacion.
        When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
        {
            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("La contrasena debe tener al menos 6 caracteres")
                .Matches("[A-Z]").WithMessage("La contrasena debe contener al menos una mayuscula")
                .Matches("[a-z]").WithMessage("La contrasena debe contener al menos una minuscula")
                .Matches("[0-9]").WithMessage("La contrasena debe contener al menos un numero")
                .Matches("[^a-zA-Z0-9]").WithMessage("La contrasena debe contener al menos un caracter especial");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Las contrasenas no coinciden");
        });
    }
}
