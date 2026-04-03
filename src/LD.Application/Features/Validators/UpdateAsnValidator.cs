using FluentValidation;
using LD.Application.Features.Asn.Commands;

namespace LD.Application.Features.Validators
{
    public class UpdateAsnValidator : AbstractValidator<UpdateAsnCommand>
    {
        public UpdateAsnValidator()
        {
            RuleFor(x => x.AsnId).GreaterThan(0).WithMessage("AsnId invalido");
            RuleFor(x => x.AsnCode).NotEmpty().WithMessage("ASN es obligatorio");
        }
    }
}
