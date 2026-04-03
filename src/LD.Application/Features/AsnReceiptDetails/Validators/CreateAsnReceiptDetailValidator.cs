using FluentValidation;
using LD.Application.Features.AsnDetail.Commands;


namespace LD.Application.Features.AsnReceiptDetails.Validators
{
    public class CreateAsnReceiptDetailValidator : AbstractValidator<CreateAsnReceiptDetailCommand>
    {
        public CreateAsnReceiptDetailValidator()
        {
            RuleFor(x => x.AsnDetailId).GreaterThan(0).WithMessage("AsnDetailId es obligatorio");
            RuleFor(x => x.PartNumber).NotEmpty().WithMessage("PartNumber es obligatorio");
        }
    }
}
