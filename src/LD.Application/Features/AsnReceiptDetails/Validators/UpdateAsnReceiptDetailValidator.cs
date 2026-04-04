using FluentValidation;
using LD.Application.Features.Asn.Commands;


namespace LD.Application.Features.AsnReceiptDetails.Validators
{
    public class UpdateAsnReceiptDetailValidator : AbstractValidator<UpdateAsnReceiptDetailCommand>
    {
        public UpdateAsnReceiptDetailValidator()
        {
            RuleFor(x => x.AsnReceiptDetailId).GreaterThan(0).WithMessage("AsnReceiptDetailId es obligatorio");
            RuleFor(x => x.PartNumber).NotEmpty().WithMessage("PartNumber es obligatorio");
        }
    }
}
