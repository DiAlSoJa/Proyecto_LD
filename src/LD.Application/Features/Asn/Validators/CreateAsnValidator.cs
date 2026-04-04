using FluentValidation;
using LD.Application.Features.Asn.Commands;
using LD.Contracts.Requests;

namespace LD.Application.Features.Asn.Validators
{
    public class CreateAsnValidator : AbstractValidator<CreateAsnCommand>
    {
        public CreateAsnValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage("Cliente es obligatorio");
            RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Proyecto es obligatorio");
            
        }
    }
}
