using FluentValidation;
using LD.Application.Features.Commands;

namespace LD.Application.Features.Asn.Validators
{
    public class UpdateAsnValidator : AbstractValidator<UpdateAsnCommand>
    {
        public UpdateAsnValidator()
        {
            RuleFor(x => x.AsnId).GreaterThan(0).WithMessage("AsnId invalido");            
        }
    }
}
