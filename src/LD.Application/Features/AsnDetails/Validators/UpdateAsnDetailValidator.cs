using FluentValidation;
using LD.Application.Features.Asn.Commands;


namespace LD.Application.Features.AsnDetails.Validators
{
    public class UpdateAsnDetailValidator : AbstractValidator<UpdateAsnDetailCommand>
    {
        public UpdateAsnDetailValidator()
        {
            RuleFor(x => x.AsnDetailId).GreaterThan(0).WithMessage("AsnDetailId es obligatorio");
            RuleFor(x => x.PartNumber).NotEmpty().WithMessage("PartNumber es obligatorio");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity debe ser mayor a 0");
        }
    }
}
