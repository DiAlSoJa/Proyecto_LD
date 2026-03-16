using FluentValidation;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Features.Projects.Comands;
using LD.Application.Features.Roles.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Roles.Validators
{
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
    {
        private readonly IApplicationUserManager _applicationUserManager;
        public UpdateRoleValidator(IApplicationUserManager applicationUserManager)
        {
            _applicationUserManager = applicationUserManager;
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del rol no puede exceder los 100 caracteres.");


            RuleForEach(x => x.Permissions)
                .MustAsync(async (permission, cancellationToken) =>
                    await _applicationUserManager.PermissionExists(permission.PermissionId.GetValueOrDefault(0)))
                .WithMessage("El permiso no existe en la base de datos");
        }


    }
}
