using FluentValidation;
using LD.Application.Features.InventoryMovement.Commands;

namespace LD.Application.Features.InventoryMovement.Validators
{
    public class CreateInventoryMovementValidator : AbstractValidator<CreateInventoryMovementCommand>
    {
        public CreateInventoryMovementValidator()
        {
            RuleFor(x => x.ClientId)
                .GreaterThan(0).WithMessage("ClientId es obligatorio");

            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("ProjectId es obligatorio");

            RuleFor(x => x.PartNumber)
                .NotEmpty().WithMessage("PartNumber es obligatorio")
                .MaximumLength(100).WithMessage("PartNumber no debe exceder 100 caracteres");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId es obligatorio");

            RuleFor(x => x.DocumentId)
                .NotEmpty().WithMessage("DocumentId es obligatorio")
                .MaximumLength(50).WithMessage("DocumentId no debe exceder 50 caracteres");

            RuleFor(x => x.Qty)
                .GreaterThanOrEqualTo(0).When(x => x.Qty.HasValue)
                .WithMessage("Qty no puede ser negativa");
        }
    }
}
