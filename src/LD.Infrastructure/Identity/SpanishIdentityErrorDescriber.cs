using Microsoft.AspNetCore.Identity;

namespace LD.Infrastructure.Identity
{
    public class SpanishIdentityErrorDescriber : IdentityErrorDescriber
    {
        private static IdentityError Error(string code, string description)
            => new()
            {
                Code = code,
                Description = description
            };

        public override IdentityError DefaultError()
            => Error(nameof(DefaultError), "Se produjo un error inesperado.");

        public override IdentityError ConcurrencyFailure()
            => Error(nameof(ConcurrencyFailure), "Error de concurrencia: el registro fue modificado por otro proceso.");

        public override IdentityError PasswordMismatch()
            => Error(nameof(PasswordMismatch), "La confirmación de la contraseña no coincide.");

        public override IdentityError InvalidToken()
            => Error(nameof(InvalidToken), "El token no es válido.");

        public override IdentityError LoginAlreadyAssociated()
            => Error(nameof(LoginAlreadyAssociated), "Ya existe una cuenta asociada con este inicio de sesión externo.");

        public override IdentityError InvalidUserName(string? userName)
            => Error(nameof(InvalidUserName), $"El nombre de usuario '{userName}' no es válido.");

        public override IdentityError InvalidEmail(string? email)
            => Error(nameof(InvalidEmail), $"El correo '{email}' no es válido.");

        public override IdentityError DuplicateUserName(string userName)
            => Error(nameof(DuplicateUserName), $"El nombre de usuario '{userName}' ya está en uso.");

        public override IdentityError DuplicateEmail(string email)
            => Error(nameof(DuplicateEmail), $"El correo '{email}' ya está en uso.");

        public override IdentityError InvalidRoleName(string? role)
            => Error(nameof(InvalidRoleName), $"El nombre del rol '{role}' no es válido.");

        public override IdentityError DuplicateRoleName(string? role)
            => Error(nameof(DuplicateRoleName), $"El rol '{role}' ya existe.");

        public override IdentityError UserAlreadyHasPassword()
            => Error(nameof(UserAlreadyHasPassword), "El usuario ya tiene una contraseña establecida.");

        public override IdentityError UserLockoutNotEnabled()
            => Error(nameof(UserLockoutNotEnabled), "El bloqueo no está habilitado para este usuario.");

        public override IdentityError UserAlreadyInRole(string role)
            => Error(nameof(UserAlreadyInRole), $"El usuario ya pertenece al rol '{role}'.");

        public override IdentityError UserNotInRole(string role)
            => Error(nameof(UserNotInRole), $"El usuario no pertenece al rol '{role}'.");

        public override IdentityError PasswordTooShort(int length)
            => Error(nameof(PasswordTooShort), $"La contraseña debe tener al menos {length} caracteres.");

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => Error(nameof(PasswordRequiresNonAlphanumeric), "La contraseña debe contener al menos un carácter especial.");

        public override IdentityError PasswordRequiresDigit()
            => Error(nameof(PasswordRequiresDigit), "La contraseña debe contener al menos un número.");

        public override IdentityError PasswordRequiresLower()
            => Error(nameof(PasswordRequiresLower), "La contraseña debe contener al menos una letra minúscula.");

        public override IdentityError PasswordRequiresUpper()
            => Error(nameof(PasswordRequiresUpper), "La contraseña debe contener al menos una letra mayúscula.");

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
            => Error(nameof(PasswordRequiresUniqueChars), $"La contraseña debe usar al menos {uniqueChars} carácter(es) distintos.");

        public override IdentityError RecoveryCodeRedemptionFailed()
            => Error(nameof(RecoveryCodeRedemptionFailed), "No se pudo canjear el código de recuperación.");
    }
}
