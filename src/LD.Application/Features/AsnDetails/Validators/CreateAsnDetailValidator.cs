using FluentValidation;
using LD.Application.Features.Asn.Commands;
using LD.Application.Features.AsnDetail.Commands;

namespace LD.Application.Features.AsnDetails.Validators
{
    public class CreateAsnDetailValidator : AbstractValidator<CreateAsnDetailCommand>
    {
        public CreateAsnDetailValidator()
        {
            RuleFor(x => x.AsnId).GreaterThan(0).WithMessage("AsnId es obligatorio");
            RuleFor(x => x.PartNumber).NotEmpty().WithMessage("PartNumber es obligatorio");
        }
    }
}
