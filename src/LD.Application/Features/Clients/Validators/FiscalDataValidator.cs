using FluentValidation;
using LD.Contracts.Requests.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Clients.Validators
{
    public class FiscalDataValidator : AbstractValidator<ClientFiscalDataRequest>
    {
        public FiscalDataValidator()
        {
            RuleFor(x => x.BusinessName)
                .NotEmpty().WithMessage("La razón social es obligatoria.")
                .MaximumLength(150).WithMessage("La razón social no puede superar 150 caracteres.");

            RuleFor(x => x.Rfc)
                .NotEmpty().WithMessage("El RFC es obligatorio.")
                .MaximumLength(20).WithMessage("El RFC no puede superar 20 caracteres.");

            RuleFor(x => x.FiscalAddress)
                .NotEmpty().WithMessage("El domicilio fiscal es obligatorio.")
                .MaximumLength(250).WithMessage("El domicilio fiscal no puede superar 250 caracteres.");

            RuleFor(x => x.Neightbourhoud)
                .NotEmpty().WithMessage("La colonia fiscal es obligatoria.")
                .MaximumLength(100).WithMessage("La colonia fiscal no puede superar 100 caracteres.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("La ciudad fiscal es obligatoria.")
                .MaximumLength(100).WithMessage("La ciudad fiscal no puede superar 100 caracteres.");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("El código postal fiscal es obligatorio.")
                .Matches(@"^\d{5}$").WithMessage("El código postal fiscal debe tener 5 dígitos.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo fiscal es obligatorio.")
                .EmailAddress().WithMessage("El correo fiscal no tiene un formato válido.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("El teléfono fiscal es obligatorio.")
                .Matches(@"^\d{10}$")
                .WithMessage("El teléfono fiscal debe tener 10 dígitos.");
        }
    }
}
