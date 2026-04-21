using FluentValidation;
using LD.Application.Features.Equipment.Commands;

namespace LD.Application.Features.Equipment.Validators;

public class CreateEquipmentValidator : AbstractValidator<CreateEquipmentCommand>
{
    public CreateEquipmentValidator()
    {
        RuleFor(x => x.EquipmentName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.SerialNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.EquipmentTypeId).GreaterThan(0);
    }
}
