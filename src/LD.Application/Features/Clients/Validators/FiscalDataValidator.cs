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
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Rfc)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.FiscalAddress)
                .NotEmpty()
                .MaximumLength(250);

            RuleFor(x => x.Neightbourhoud)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.ZipCode)
                .NotEmpty()
                .Matches(@"^\d{5}$");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Phone)
                .NotEmpty()
                .Matches(@"^\d{10}$")
                .WithMessage("El teléfono fiscal debe tener 10 dígitos.");
        }
    }
}
